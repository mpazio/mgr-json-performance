﻿using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace JSONPerformance.Databases;

public class Oracle : Database
{
    private OracleConnection? _connection;
    
    public Oracle(string connectionString) : base(connectionString)
    {
        
    }

    public override async Task Connect()
    {
        _connection = new OracleConnection(ConnectionString);
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
        await using (var cmd = new OracleCommand())
        {
            cmd.Connection = _connection;
            foreach (var query in data)
            {
                cmd.CommandText = query;
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public override Task Truncate(string tableName, params string[]? parameters)
    {
        throw new NotImplementedException();
    }

    public override async Task ExecuteQuery(string query)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new OracleCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = query;

            await cmd.ExecuteNonQueryAsync();
        }
    }
}