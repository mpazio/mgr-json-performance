using Oracle.ManagedDataAccess.Client;

namespace JSONPerformance.Databases;

public class Oracle : Database
{
    private OracleConnection _connection;
    
    public Oracle(string connectionString) : base(connectionString)
    {
        
    }

    public override async Task Connect()
    {
        _connection = new OracleConnection(ConnectionString);
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