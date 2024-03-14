using System.Data;
using Microsoft.Data.SqlClient;

namespace JSONPerformance.Databases;

public class SqlServer : Database
{
    private SqlConnection? _connection;
    
    public SqlServer(string connectionString) : base(connectionString)
    {
        
    }
    
    public override async Task Connect()
    {
        _connection = new SqlConnection(ConnectionString);
        await _connection.OpenAsync();
    }

    public override Task<bool> IsConnected()
    {
        if (_connection is null) return Task.FromResult(false);
        switch (_connection.State)
        {
            case ConnectionState.Closed:
            case ConnectionState.Broken:
                return Task.FromResult(false);
            default:
                return Task.FromResult(true);
        }
    }

    public override async Task SeedDatabase(string[] data, params string[]? parameters)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new SqlCommand())
        {
            cmd.Connection = _connection;
            foreach (var query in data)
            {
                cmd.CommandText = query;
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new SqlCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = $"TRUNCATE TABLE {tableName}";
            
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public override async Task ExecuteQuery(string query)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new SqlCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = query;

            await cmd.ExecuteNonQueryAsync();
        }
    }
}