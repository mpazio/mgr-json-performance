using MongoDB.Driver;

namespace JSONPerformance.Databases;

public class Mongo: Database
{
    private MongoClient _client;
    
    public Mongo(string connectionString) : base(connectionString)
    {
        _client = new MongoClient(connectionString);
    }
    
    public override async Task Connect()
    {
        _client = new MongoClient(ConnectionString);
    }

    public override async Task<bool> IsConnected()
    {
        try
        {
            var ping = _client.ListDatabaseNames().FirstOrDefault();
            return ping is not null;
        }
        catch (Exception)
        {
            return false;
        }
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