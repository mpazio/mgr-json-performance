namespace JSONPerformance.Contracts;

public interface IDatabase
{
    Task Connect();
    Task<bool> IsConnected();
    Task SeedDatabase(string[] data, params string[]? parameters);
    Task ExecuteQuery(string query);
}