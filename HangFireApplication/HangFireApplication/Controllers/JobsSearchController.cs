namespace HangFireApplication.Controllers;

public class JobsSearchController : Controller
{
    #region Ctor 
    private readonly IBackgroundJobClient _client;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRedisService<JobSearch> _redisService;
    public JobsSearchController(
        IBackgroundJobClient client,
        IPublishEndpoint publishEndpoint,
        IRedisService<JobSearch> redisService)
    {
        _client = client;
        _redisService = redisService;
        _publishEndpoint = publishEndpoint;
    } 
    #endregion

    public IActionResult Index() => View();

    public IActionResult Search() 
    {
        var data = _redisService.Get(Consts.RedisConsts.JOBSEARCH_KEY);


        return Json(
            new
            {
                Status = HttpStatusCode.OK,
                Data = data,
                Message = "islem basarili bir sekilde gerceklesti"

            });
    }

    [HttpPost]
    public IActionResult Search([FromBody] JobSearch model)
    {

        if(ModelState.IsValid)
        {
            if (model.SearchNow)
            {
                foreach (var request in model.Companies)
                {
                    foreach (var keyword in model.KeyWords)
                    {
                        var message = new JobSearchDto
                        {
                            KeyWord = keyword,
                            WebUrl = request
                        };
        
                        _client.Enqueue(() => SendJob(message));
                    }
                }
            }
            else if(!model.SearchNow && model.ScheduleTime != null && model.ScheduleTime > DateTime.Now)
            {
                foreach (var request in model.Companies)
                {
                    foreach (var keyword in model.KeyWords)
                    {
                        var message = new JobSearchDto
                        {
                            KeyWord = keyword,
                            WebUrl = request
                        };

                        _client.Schedule(() => SendJob(message), model.ScheduleTime.Value);                
                    }
                }
            }
        }

        _redisService.Sets(Consts.RedisConsts.JOBSEARCH_KEY, model);


        return Json(
            new
            {
                Status = HttpStatusCode.OK,
                Data = model,
                Message = "islem basarili bir sekilde gerceklesti"

            });
    }

    [NonAction]
    public async Task SendJob(JobSearchDto model)
    {
        await _publishEndpoint.Publish(model);
    }
}
