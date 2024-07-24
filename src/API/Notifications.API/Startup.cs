using Microsoft.Extensions.Diagnostics.HealthChecks;
using Notifications.GrpcServer.Services;
using ProtoBuf.Grpc.Server;

namespace Notifications.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DB Connection
            string defaultConnection = Environment.GetEnvironmentVariable("CONNECTION_STRINGS") ?? Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            //services.AddDbContext<AppDbContext>(context => context.UseNpgsql(defaultConnection));

            //Services
            services.AddCodeFirstGrpc();
            services.AddCodeFirstGrpcReflection();
            services.AddNotificationsGrpcServerServices();

            services.AddMessageProcessorServices();

            //Rafael
            //Indica que estou a trabalhar com a arquitetura MVC com Views Controllers. Permite chamar o meu controller
            //NewsoftJson para evitar um loop infinito no retorno
            //AddJsonOptions para os Enums, onde para cada item do meu Enum, retorna um Id
            services.AddControllers()
                    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            //Rafael AutoMapper
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Notifications.API",
                    Description = "An ASP.NET Core Web API for Notifications",
                    Contact = new OpenApiContact()
                    {
                        Name = "Auto Moreira Portugal",
                        Email = "automoreiraportugal@gmail.com"
                    },
                    Version = "v1"
                });
            });

            //CORS - Dado qualquer header da requisição por http vinda de qualquer metodo (get, post, delete..) e vindos de qualquer origem
            services.AddCors(o => o.AddPolicy("CustomPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Token-Expired"); ;
            }));

            List<string> tagsListGrpc = new()
            {
                "Internal Services",
                "Grpc"
            };
            services.AddGrpcHealthChecks().AddCheck("NotificationsGrpc", () => HealthCheckResult.Healthy(), tagsListGrpc);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //CORS
            app.UseCors("CustomPolicy");

            //app.AddHealthCheckApp();

            //Update Database
            /*using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                Console.WriteLine("Update database started");
                context.Database.SetCommandTimeout(TimeSpan.FromHours(2));
                context.Database.EnsureCreated(); //Migrate();
                Console.WriteLine("Update database ended");
            }*/

            //GraphQL
            /* app.UsePlayground(new PlaygroundOptions
             {
                 QueryPath = "/graphql",
                 Path = "/playground"
             }); */

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoMoreira.API v1"));
            app.UseWelcomePage(new WelcomePageOptions { Path = new PathString("/swagger/index.html") });


            //HTTPS
            //app.UseHttpsRedirection();

            //Indica que vou trabalhar por rotas.
            app.UseRouting();
            app.UseWebSockets();

            //E vou retornar determinados endpoints de acordo com a configuração destas rotas dentro do meu controller
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCodeFirstGrpcReflectionService();
                endpoints.MapGrpcService<NotificationsGrpcServerService>();
                endpoints.MapGrpcHealthChecksService();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("There is http and gRPC communication endpoints.");
                });
                endpoints.MapControllers();
                //endpoints.MapGraphQL();
            });
        }
    }
}
