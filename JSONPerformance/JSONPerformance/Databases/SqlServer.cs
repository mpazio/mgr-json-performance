using Microsoft.Data.SqlClient;

namespace JSONPerformance.Databases;

public class SqlServer : Database
{
    private SqlConnection _connection;
    
    public SqlServer(string connectionString) : base(connectionString)
    {
        
    }
    
    public override async Task Connect()
    {
        _connection = new SqlConnection(ConnectionString);
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