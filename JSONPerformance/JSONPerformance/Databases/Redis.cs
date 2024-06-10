using System.Text.Json;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Aggregation;
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
        ConfigurationOptions co = new ConfigurationOptions()
        {
            SyncTimeout = 500000,
            EndPoints = new EndPointCollection()
            {
                ConnectionString
            }
        };
        _redis = await ConnectionMultiplexer.ConnectAsync(co);
        Console.WriteLine(_redis.TimeoutMilliseconds);
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

    public override async Task ExecuteQuery(string query, params string[]? parameters)
    {
        if (parameters is null || parameters.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(parameters));
        
        var index = parameters[0];
        var returnFieldsParam = parameters[1];
        var sortParam = (parameters.Length == 3) ? parameters[2] : "";
        var returnFields = returnFieldsParam.Split(" ");

        // var res = await _ft.AggregateAsync(index, new AggregationRequest(query).GroupBy("@locationCountry", new Reducer[] { Reducers.Count()}));
        // Console.WriteLine(res.TotalResults);
        ThreadPool.SetMinThreads(10, 10);
        if (sortParam == "")
        {
            var res = await _ft.SearchAsync(index, new Query(query).Limit(0, 1000000).ReturnFields(returnFields).Timeout(10000));
            var documents = res.Documents.Select(x => x["json"]);
            Console.WriteLine(documents.Count());
        }
        else
        {
            var res = await _ft.SearchAsync(index, new Query(query).Limit(0, 1000000).SetSortBy("locationCountry").ReturnFields(returnFields).Timeout(1000000));
            var documents = res.Documents.Select(x => x["json"]);
            Console.WriteLine(documents.Count());
        }


        // foreach (var doc in documents)
        // {
        //     Console.WriteLine(doc.ToString());
        // }
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