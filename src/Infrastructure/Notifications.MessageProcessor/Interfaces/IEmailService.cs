namespace Notifications.MessageProcessor.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string toName, string toAddress, string password);
        Task SendUserProfileUpdatedEmailAsync(string toName, string toAddress);
        Task SendClientEmailAsync(string toName, string toAddress);
        Task SendPasswordChangedEmailAsync(string toName, string toAddress, string password);
        Task SendPasswordResetEmailAsync(string toName, string toAddress, string password);
    }
}
