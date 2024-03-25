using Couchbase;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace JSONPerformance.Databases;

public class Couchbase : Database
{
    private ICluster _cluster;
    public Couchbase(string connectionString) : base(connectionString)
    {
    }

    public override async Task Connect()
    {
        _cluster = await Cluster.ConnectAsync(ConnectionString, new ClusterOptions
        {
            UserName = "Administrator",
            Password = "password",
        });
    }

    public override async Task<bool> IsConnected()
    {
        try
        {
            var bucket = await _cluster.Buckets.GetBucketAsync("data");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override async Task SeedDatabase(string[] data, params string[]? parameters)
    {
        var bucket = await _cluster.BucketAsync("data");
        var scope = await bucket.ScopeAsync("_default");
        var collection = await scope.CollectionAsync("_default");

        for (int i = 0; i < data.Length; i++)
        {
            dynamic v = JObject.Parse(data[i]);
            Console.WriteLine(v);
            
            await collection.InsertAsync<dynamic>(i.ToString(), v);
        }
        
    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        await _cluster.QueryAsync<BsonDocument>($"DELETE FROM {tableName}");
    }

    public override async Task ExecuteQuery(string query)
    {
        var results = await _cluster.QueryAsync<dynamic>(query);
    }
}