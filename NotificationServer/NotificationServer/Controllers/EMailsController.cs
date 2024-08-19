using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationServer.Models;
using NotificationServer.Services;

namespace NotificationServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EMailsController : ControllerBase
{
    private readonly IMailService _mailService;

    public EMailsController(IMailService mailService)
    {
        _mailService = mailService;
    }
    [HttpPost]
    public async Task<IActionResult> SendMail([FromBody] EmailBody request)
    {
        try
        {
            await _mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
