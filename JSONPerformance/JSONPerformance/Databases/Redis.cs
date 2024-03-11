using StackExchange.Redis;

namespace JSONPerformance.Databases;

public class Redis : Database
{
    private ConnectionMultiplexer _redis;
    public Redis(string connectionString) : base(connectionString)
    {
        
    }

    public override async Task Connect()
    {
        _redis = await ConnectionMultiplexer.ConnectAsync(ConnectionString);
    }

    public override Task<bool> IsConnected()
    {
        throw new NotImplementedException();
    }

    public override Task SeedDatabase(string[] data)
    {
        throw new NotImplementedException();
    }

    public override Task ExecuteQuery(string query)
    {
        throw new NotImplementedException();
    }
}