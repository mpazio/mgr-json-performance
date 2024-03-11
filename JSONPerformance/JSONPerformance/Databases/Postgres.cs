using Npgsql;

namespace JSONPerformance.Databases;

public class Postgres : Database
{
    private readonly NpgsqlDataSource _dataSource;
    private NpgsqlConnection? _connection;
    
    public Postgres(string connectionString) : base(connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        _dataSource = dataSourceBuilder.Build();
    }
    
    public override Task Connect()
    {
        throw new NotImplementedException();
    }

    public override Task<bool> IsConnected()
    {
        throw new NotImplementedException();
    }

    public override Task SeedDatabase(string[] data)
    {
        throw new NotImplementedException();
    }

    public override Task ExecuteQuery(string query)
    {
        throw new NotImplementedException();
    }
}