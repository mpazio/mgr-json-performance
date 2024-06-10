namespace JSONPerformance.Contracts;

public interface IDatabase
{
    Task Connect();
    Task<bool> IsConnected();
    Task SeedDatabase(string[] data, params string[]? parameters);
    Task Truncate(string tableName, params string[]? parameters);
    Task ExecuteQuery(string query, params string[]? parameters);
    Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters);
}