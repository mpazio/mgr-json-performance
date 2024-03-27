using System.Text.Json;
using MongoDB.Bson;
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
        catch (Exception ex)
        {
            Console.WriteLine("Ex"+ ex);
            return false;
        }
    }

    public override async Task SeedDatabase(string[] data, params string[]? parameters)
    {
        var db = Client.GetDatabase("test");
        var collections = (await db.ListCollectionNamesAsync()).ToList();
        var isThereJsonData = collections.Any(e => e.Equals("jsondata"));
        if(!isThereJsonData){
        {
            await db.CreateCollectionAsync("jsondata");
        }}

        var collection = db.GetCollection<BsonDocument>("jsondata");

        List<BsonDocument> bsonData = new List<BsonDocument>();
        foreach (var d in data)
        {
            bsonData.Add(BsonDocument.Parse(d));
        }
        
        await collection.InsertManyAsync(bsonData);

    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        var db = Client.GetDatabase("test");
        await db.DropCollectionAsync(tableName);
    }

    public override async Task ExecuteQuery(string query)
    {
        var db = Client.GetDatabase("test");
        var bsonQuery = new JsonCommand<BsonDocument>(query);
        await db.RunCommandAsync(bsonQuery);
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        if (parameters.Length != 1)
            throw new ArgumentOutOfRangeException(nameof(parameters));

        var databaseName = parameters[0];
        
        var db = Client.GetDatabase(databaseName);
        var bsonQuery = new JsonCommand<BsonDocument>(query);
        var res = await db.RunCommandAsync(bsonQuery);

        // var jsonRes = JsonSerializer.Serialize(res, new JsonSerializerOptions()
        // {
        //     WriteIndented = true,
        //     PropertyNameCaseInsensitive = true
        // });
        
        return res.ToString();
    }
}