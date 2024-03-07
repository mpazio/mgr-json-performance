namespace JSONPerformance;

public interface IDatabase
{
    Task Connect();
    Task<bool> IsConnected();
    Task SeedDatabase(string[] data);
    Task ExecuteQuery(string query);
}