using Newtonsoft.Json;
using StackExchange.Redis;

namespace HangFireApplication.Services;

public interface IRedisCache { }

public interface IRedisService<T> 
    where T : IRedisCache
{
    IEnumerable<T> Get(string key);
    void Delete(string key);
    void Set(string key, T value);
    void Sets(string key, T value);
}

public class RedisService<T> : IRedisService<T> where T : IRedisCache
{
    #region Ctor 
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _redis;

    public RedisService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
        _redis = redis;
    } 
    #endregion

    public void Delete(string key)
    {
        if(string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        _database.KeyDelete(key);
    }

    public IEnumerable<T> Get(string key)
    {
        var jsonData = _database.StringGet(key);
        if (jsonData.IsNullOrEmpty)
            return default;

        return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonData);
    }

    public void Set(string key, T value)
    {
        if(string.IsNullOrWhiteSpace(key)) 
            throw new ArgumentNullException(nameof(key));
        var jsonData = JsonConvert.SerializeObject(value);
        _database.StringSet(key, jsonData);
    }

    public void Sets(string key, T value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var jsonData = JsonConvert.SerializeObject(value);
        _database.ListRightPush(key, jsonData);
    }
} 