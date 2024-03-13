using Couchbase;

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

    public override Task SeedDatabase(string[] data, params string[]? parameters)
    {
        throw new NotImplementedException();
    }

    public override async Task ExecuteQuery(string query)
    {
        var statement = "SELECT * FROM `data`";

        var results = await _cluster.QueryAsync<dynamic>(statement);

        await foreach (var result in results) Console.WriteLine(result);
    }
}