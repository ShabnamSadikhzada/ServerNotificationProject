using NotificationServer.Models;

namespace NotificationServer.Services;

public interface IMailService
{
    Task SendEmailAsync(EmailBody email);
}
