namespace Notifications.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await NotificationsGrpcBuilder.CreateNotificationsGrpcProtoFileAsync();
            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;
                AppDbContext context = services.GetRequiredService<AppDbContext>();
                Console.WriteLine("Update database started");
                context.Database.SetCommandTimeout(TimeSpan.FromHours(2));
                context.Database.EnsureCreated(); //Migrate();
                Console.WriteLine("Update database ended");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                    webBuilder.SetKestrelOptions(false, true);
                });
    }
}
