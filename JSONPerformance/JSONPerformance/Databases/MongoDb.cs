using MongoDB.Driver;

namespace JSONPerformance.Databases;

public class MongoDb: Database
{
    public IMongoClient Client { get; set; }

    public MongoDb(string connectionString) : base(connectionString)
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

    public override Task SeedDatabase(string[] data, params string[]? parameters)
    {
        throw new NotImplementedException();
    }

    public override Task Truncate(string tableName, params string[]? parameters)
    {
        throw new NotImplementedException();
    }

    public override Task ExecuteQuery(string query)
    {
        throw new NotImplementedException();
    }
}