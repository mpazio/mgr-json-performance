using CouchDB.Client;
using CouchDB.Client.FluentMango;

namespace JSONPerformance.Databases;

public class CouchDb : Database
{
    private CouchClient _client;
    private readonly HttpClient _httpClient;

    public CouchDb(string connectionString) : base(connectionString)
    {
        _httpClient = new HttpClient();
    }

    public override async Task Connect()
    {
        _client = new CouchClient(ConnectionString);
    }

    public override async Task<bool> IsConnected()
    {
        try
        {
            await _client.ListAllDatabasesAsync();
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
        var db = await _client.GetDatabaseAsync("test");
        try
        {
            await _httpClient.PostAsync($"{ConnectionString}/test/_find",
                new StringContent(query));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}