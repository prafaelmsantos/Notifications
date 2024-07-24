namespace Notifications.MessageProcessor.ServicesRegistry
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMessageProcessorServices(this IServiceCollection services)
        {
            return services.AddMessageProcessorServices(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        }
        public static IServiceCollection AddMessageProcessorServices(this IServiceCollection services, IConfiguration configuration)
        {
            string name = Environment.GetEnvironmentVariable("EMAIL_NAME") ?? configuration?.GetValue<string>("EmailSettings:Name") ?? "Auto Moreira Portugal";
            string address = Environment.GetEnvironmentVariable("EMAIL_ADDRESS") ?? configuration?.GetValue<string>("EmailSettings:Address") ?? "automoreiraportugal@gmail.com";
            string username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? configuration?.GetValue<string>("EmailSettings:Username") ?? "automoreiraportugal@gmail.com";
            string password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? configuration?.GetValue<string>("EmailSettings:Password") ?? "vwlv dhah uzfu ijvi";
            string host = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? configuration?.GetValue<string>("EmailSettings:Host") ?? "smtp.gmail.com";

            bool validPort = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT"), out var envPort);
            int port = validPort ? envPort : configuration?.GetValue<int>("EmailSettings:Port") ?? 465;

            EmailConfig emailConfig = new()
            {
                Name = name,
                Address = address,
                Username = username,
                Password = password,
                Host = host,
                Port = port
            };

            #region Services

            services.AddScoped<IEmailService, EmailService>(x => new EmailService(emailConfig));

            return services;

            #endregion
        }
    }
}
