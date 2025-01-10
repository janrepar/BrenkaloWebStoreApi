using MimeKit;

namespace BrenkaloWebStoreApi.Services
{
    public interface IEmailService
    {
        Task SendMessageAsync(MimeMessage message);
        MimeMessage BuildMessageBody(string recipient, string newPassword, string subject = "Password Reset Notification");
    }
}
