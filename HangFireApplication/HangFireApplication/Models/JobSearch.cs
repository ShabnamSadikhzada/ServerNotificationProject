using HangFireApplication.Services;
using System.Net;

namespace HangFireApplication.Models;

public class JobSearch : IRedisCache
{
    public JobSearch()
    {
        CreatedData = DateTime.UtcNow;
        SearchBy = Dns.GetHostName();
    }


    public string[]? KeyWords { get; set; }
    public string[]? Companies { get; set; }
    public bool SearchNow { get; set; }
    public DateTime? ScheduleTime { get; set; }

    public DateTime? CreatedData { get; private set; }
    public string? SearchBy { get; private set; }

}
