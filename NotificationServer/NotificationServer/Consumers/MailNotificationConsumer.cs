using MassTransit;
using NotificationServer.Services;
using Shared.Dtos.Emails;

namespace NotificationServer.Consumers;

public class MailNotificationConsumer : IConsumer<EmailBodyDto>
{
    private readonly IMailService _mailService;

    public MailNotificationConsumer(IMailService mailService)
    {
        _mailService = mailService;
    }

    public async Task Consume(ConsumeContext<EmailBodyDto> context)
    {
        await _mailService.SendEmailAsync(context.Message);
    }
}
