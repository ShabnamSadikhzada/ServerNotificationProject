using Hangfire;
using HangFireApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;
namespace HangFireApplication.Controllers;

public class EmailController : Controller
{
    private readonly IBackgroundJobClient _client;

    public EmailController(IBackgroundJobClient client)
    {
        _client = client;
    }


    public IActionResult Send() => View();

    [HttpPost]
    public IActionResult Send(EmailBodyDto model)
    {
        if (model.SendNow)
        {
            _client.Enqueue(()  => SendEmailJobAsync(model));
        }
        else
        {
            var timeUntilSendDate = model.ScheduleTime.Value - DateTime.Now;
            _client.Schedule(() => SendEmailJobAsync(model), timeUntilSendDate);
        }


        return View(model);
    }

    [NonAction]
    public async Task SendEmailJobAsync (EmailBodyDto request)
    {
        var mailServiceApi = RestService.For<IMailServiceApi>("http://localhost:5020");
        await mailServiceApi.SendEmailAsync(request);
    }

}
