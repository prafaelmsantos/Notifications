namespace Notifications.GrpcServer.ServicesRegistry
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotificationsGrpcServerServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationsGrpcServerService, NotificationsGrpcServerService>();
            return services;
        }
    }
}
