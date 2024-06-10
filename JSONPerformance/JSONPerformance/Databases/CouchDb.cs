using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using CouchDB.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JSONPerformance.Databases;

public class CouchDb : Database
{
    private CouchClient _client;
    private readonly HttpClient _httpClient;

    public CouchDb(string connectionString) : base(connectionString)
    {
        _httpClient = new HttpClient() {Timeout = TimeSpan.FromSeconds(1000)};
        Uri baseUri = new Uri(connectionString);
        _httpClient.BaseAddress = baseUri;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.ConnectionClose = true;
    }

    public override Task Connect()
    {
        return Task.CompletedTask;
    }

    public override async Task<bool> IsConnected()
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ConnectionString);
        request.AllowAutoRedirect = false;
        request.Method = "HEAD";
        try {
            request.GetResponse();
            return true;
        } catch (WebException wex)
        {
            return false;
        }
    }

    public override async Task SeedDatabase(string[] data, params string[]? parameters)
    {
        var base64EncodedAuthenticationString = GetAuthenticationStringInBase64(parameters);
        var serializedData = ParseData(data);
        try
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/test/_bulk_docs");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = new StringContent(serializedData,
                Encoding.UTF8, 
                "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        var base64EncodedAuthenticationString = GetAuthenticationStringInBase64(parameters);
        try
        {
            var deleteMessage = new HttpRequestMessage(HttpMethod.Delete, "/test");
            deleteMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            var deleteResponse = await _httpClient.SendAsync(deleteMessage);
            deleteResponse.EnsureSuccessStatusCode();
            
            var putMessage = new HttpRequestMessage(HttpMethod.Put, "/test");
            putMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            var putResponse = await _httpClient.SendAsync(putMessage);
            putResponse.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task ExecuteQuery(string query, params string[]? parameters)
    {
        var base64EncodedAuthenticationString = GetAuthenticationStringInBase64(new string[]{"admin", "password"});
        try
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/test/_find");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = new StringContent(query,
                Encoding.UTF8, 
                "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        var base64EncodedAuthenticationString = GetAuthenticationStringInBase64(new string[]{"admin", "password"});
        try
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/test/_find");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = new StringContent(query,
                Encoding.UTF8, 
                "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string ParseData(string[] data)
    {
        string parsedData = "";
        foreach (var query in data)
        {
            parsedData += query+",\n";
        }

        parsedData = parsedData.Substring(0, parsedData.Length - 2);
        string serializedData = "{ \"docs\" : [" + parsedData + "]}";
        return serializedData;
    }
    
    private static string GetAuthenticationStringInBase64(string[]? parameters)
    {
        if (parameters is null || parameters.Length < 1)
        {
            throw new Exception();
        }
        
        if (parameters.Length != 2)
        {
            throw new Exception();
        }
        
        var admin = parameters[0];
        var password = parameters[1];
        var authenticationString = $"{admin}:{password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        return base64EncodedAuthenticationString;
    }
}