using JSONPerformance.Contracts;

namespace JSONPerformance.Databases;

public abstract class Database(string connectionString) : IDatabase
{
    private IDatabase _databaseImplementation;
    public string ConnectionString { get; } = connectionString;

    public abstract Task Connect();
    public abstract Task<bool> IsConnected();
    public abstract Task SeedDatabase(string[] data, params string[]? parameters);
    public abstract Task Truncate(string tableName, params string[]? parameters);
    public abstract Task ExecuteQuery(string query);
    public abstract Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters);
}