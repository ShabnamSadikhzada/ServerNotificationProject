using Hangfire;
using HangFireApplication.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HangFireApplication.Controllers;

public class JobsSearchController : Controller
{
    private readonly IBackgroundJobClient _client;
    private readonly IPublishEndpoint _publishEndpoint;
    public JobsSearchController(IBackgroundJobClient client, IPublishEndpoint publishEndpoint)
    {
        _client = client;
        _publishEndpoint = publishEndpoint;
    }

    public IActionResult Index() => View();

    public IActionResult Search() => View();

    [HttpPost]
    public IActionResult Search([FromBody] JobSearch model)
    {
        if(ModelState.IsValid)
        {
            if (model.SearchNow)
            {
                _client.Enqueue(() => SendJob(model));
            }
            else if(!model.SearchNow && model.ScheduleTime != null && model.ScheduleTime > DateTime.Now)
            {
                 _client.Schedule(() => SendJob(model), model.ScheduleTime.Value);
            }
        }

        return Json(HttpStatusCode.OK);
    }

    public async Task SendJob(JobSearch model)
    {
        await _publishEndpoint.Publish(model);
    }
}
