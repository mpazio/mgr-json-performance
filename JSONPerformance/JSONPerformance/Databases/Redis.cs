using System.Text.Json;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;

namespace JSONPerformance.Databases;

public class Redis : Database
{
    private ConnectionMultiplexer _redis;
    private IDatabase _db;
    private SearchCommands _ft;
    public Redis(string connectionString) : base(connectionString)
    {
        
    }

    public override async Task Connect()
    {
        _redis = await ConnectionMultiplexer.ConnectAsync(ConnectionString);
        _db = _redis.GetDatabase();
        _ft = _db.FT();
    }

    public override Task<bool> IsConnected()
    {
        return Task.FromResult(_redis.IsConnected);
    }

    public override Task SeedDatabase(string[] data, params string[]? parameters)
    {
        if (parameters is null || parameters.Length < 1)
        {
            throw new Exception();
        }
        
        if (parameters.Length != 2)
        {
            throw new Exception();
        }
        
        var json = _db.JSON();
        var index = 1;
        var indexName = parameters[0];
        var path = parameters[1];

        foreach (var insert in data)
        {
            json.Set($"{indexName}:{index++}", path, insert);
        }
        return Task.FromResult(true);
    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        await _db.ExecuteAsync("FLUSHDB");
    }

    public override async Task ExecuteQuery(string query)
    {
        var res = await _ft.SearchAsync("userIndex", new Query(query));
        var documents = res.Documents.Select(x => x["json"]);
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        if (parameters.Length != 1)
            throw new ArgumentOutOfRangeException(nameof(parameters));

        var index = parameters[0];
        
        var res = await _ft.SearchAsync(index, new Query(query));
        var documents = res.Documents.Select(x =>
        {
            var props = x.GetProperties();
            var doc = "";
            foreach (var keyValuePair in props)
            {
                doc = x[keyValuePair.Key];
            }
            return doc;
        });
        
        var jsonRes = JsonSerializer.Serialize(documents, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        });

        return jsonRes;
    }
}