using MailKit.Net.Smtp;
using MimeKit;
using NotificationServer.Configurations;
using NotificationServer.Models;

namespace NotificationServer.Services;

public class MailService : IMailService
{
    #region Constructor
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    } 
    #endregion

    public async Task SendEmailAsync(EmailBody email)
    {
        var configuration = _configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(configuration.DisplayName, configuration.From));
        emailMessage.To.Add(new MailboxAddress("n7had", email.To));

        if(!string.IsNullOrEmpty(email.Cc))
        {
            emailMessage.Cc.Add(new MailboxAddress("Naz Cc", email.Cc));
        }
        
        if(!string.IsNullOrEmpty(email.Bcc))
        {
            emailMessage.Bcc.Add(new MailboxAddress("Haciyev Bcc", email.Bcc));
        }

        emailMessage.Subject = email.Subject;
        var bodyBuiler = new BodyBuilder
        {
            HtmlBody = email.Body
        };

        if (email.Attachments.Count > 0)
        {
            foreach (var attachment in email.Attachments)
            {
                bodyBuiler.Attachments.Add(attachment.FileName, attachment.FileContent);
            }
        }
        emailMessage.Body = bodyBuiler.ToMessageBody();

        using(var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(configuration.SmtpServer, configuration.Port, false);
                await client.AuthenticateAsync(configuration.Username, configuration.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}