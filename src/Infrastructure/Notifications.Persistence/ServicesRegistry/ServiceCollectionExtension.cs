namespace Notifications.Persistence.ServicesRegistry
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            #region Repositories
            services.AddScoped<IClientMessageRepository, ClientMessageRepository>();
            services.AddScoped<IVisitorRepository, VisitorRepository>();
            #endregion

            #region Services
            services.AddScoped<IClientMessageService, ClientMessageService>();
            services.AddScoped<IVisitorService, VisitorService>();
            #endregion

            return services;
        }
    }
}
