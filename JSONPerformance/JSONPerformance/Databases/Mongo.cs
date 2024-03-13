using MongoDB.Driver;

namespace JSONPerformance.Databases;

public class Mongo: Database
{
    public IMongoClient Client { get; set; }

    public Mongo(string connectionString) : base(connectionString)
    {
        Client = new MongoClient(connectionString);
    }
    
    public override async Task Connect()
    {
        Client = new MongoClient(ConnectionString);
    }

    public override async Task<bool> IsConnected()
    {
        try
        {
            await Client.ListDatabaseNamesAsync();
            return true;
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