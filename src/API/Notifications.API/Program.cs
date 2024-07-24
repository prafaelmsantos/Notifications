using Base.Lib.Shared.HostSetup;
using Notifications.GrpcServer.Builders;

namespace Notifications.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await NotificationsGrpcBuilder.CreateNotificationsGrpcProtoFileAsync();
            await CreateHostBuilder(args).Build().RunAsync();
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
