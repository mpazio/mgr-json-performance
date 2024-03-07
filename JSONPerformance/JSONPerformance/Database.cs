namespace JSONPerformance;

public abstract class Database : IDatabase
{
    protected Database(string connectionString)
    {
        
    }

    public abstract Task Connect();
    public abstract Task<bool> IsConnected();
    public abstract Task SeedDatabase(string[] data);
    public abstract Task ExecuteQuery(string query);
}