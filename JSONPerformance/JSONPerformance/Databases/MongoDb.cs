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
        if (parameters.Length != 2)
            throw new ArgumentOutOfRangeException(nameof(parameters));
        
        var dbParam = parameters[0];
        var collectionName = parameters[1];
        
        var db = Client.GetDatabase(dbParam);
        var collections = (await db.ListCollectionNamesAsync()).ToList();
        var isThereJsonData = collections.Any(e => e.Equals(collectionName));
        if(!isThereJsonData){
        {
            await db.CreateCollectionAsync(collectionName);
        }}

        var collection = db.GetCollection<BsonDocument>(collectionName);

        List<BsonDocument> bsonData = new List<BsonDocument>();
        foreach (var d in data)
        {
            bsonData.Add(BsonDocument.Parse(d));
        }
        
        await collection.InsertManyAsync(bsonData);

    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        if (parameters.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(parameters));
        
        var dbParam = parameters[0];
        
        var db = Client.GetDatabase(dbParam);
        await db.DropCollectionAsync(tableName);
    }

    public override async Task ExecuteQuery(string query, params string[]? parameters)
    {
        if (parameters.Length < 1)
            throw new ArgumentOutOfRangeException(nameof(parameters));
        
        var dbParam = parameters[0];
        
        var db = Client.GetDatabase(dbParam);
        var bsonQuery = new JsonCommand<BsonDocument>(query);
        var res = db.RunCommandAsync(bsonQuery).ToAsyncEnumerable();
        var e = res.GetAsyncEnumerator();
        try
        {
            while (await e.MoveNextAsync())
            {
                // Console.WriteLine(e.Current.ToString());
            };
        }
        finally { if (e != null) await e.DisposeAsync(); }
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        if (parameters.Length != 1)
            throw new ArgumentOutOfRangeException(nameof(parameters));

        var databaseName = parameters[0];
        
        var db = Client.GetDatabase(databaseName);
        var bsonQuery = new JsonCommand<BsonDocument>(query);
        var res = await db.RunCommandAsync(bsonQuery);
        
        return res.ToString();
    }
}