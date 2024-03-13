using Couchbase;

namespace JSONPerformance.Databases;

public class CouchDb : Database
{
    private ICluster _cluster;
    public CouchDb(string connectionString) : base(connectionString)
    {
        
    }

    public override async Task Connect()
    {
        _cluster = await Cluster.ConnectAsync(ConnectionString);
    }

    public override Task<bool> IsConnected()
    {
        throw new NotImplementedException();
    }

    public override Task SeedDatabase(string[] data, params string[]? parameters)
    {
        throw new NotImplementedException();
    }

    public override Task ExecuteQuery(string query)
    {
        throw new NotImplementedException();
    }
}