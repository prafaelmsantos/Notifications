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
            services.AddDbContext<AppDbContext>(context => context.UseNpgsql(defaultConnection));

            //Services
            services.AddCodeFirstGrpc();
            services.AddCodeFirstGrpcReflection();
            services.AddNotificationsGrpcServerServices();

            services.AddGraphQLServices();
            services.AddMessageProcessorServices();
            services.AddPersistenceServices();

            services.AddControllers()
                    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
                    Version = "1.0"
                });
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("X-Version"));
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            //Cors
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

            //GraphQL
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql",
                Path = "/playground"
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications.API v1"));

            //HTTPS
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseWebSockets();
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
                endpoints.MapGraphQL();
            });
        }
    }
}
