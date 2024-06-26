﻿using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace JSONPerformance.Databases;

public class Oracle : Database
{
    private OracleConnection? _connection;
    
    public Oracle(string connectionString) : base(connectionString)
    {
        _connection = new OracleConnection(ConnectionString);
    }

    public override async Task Connect()
    {
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

    public override async Task Truncate(string tableName, params string[]? parameters)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new OracleCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = $"TRUNCATE TABLE {tableName}";
            
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public override async Task ExecuteQuery(string query , params string[]? parameters)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new OracleCommand())
        {
            Console.WriteLine(query);
            cmd.Connection = _connection;
            cmd.CommandText = query;

            var res  = await cmd.ExecuteReaderAsync();
            while (await res.ReadAsync())
            {
                
            }
        }
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        if (!await IsConnected()) return "";

        string res = "";
        await using (var cmd = new OracleCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = query;

            var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    res += reader.GetValue(i) + " ";
                }
                
            }
        }

        return res;
    }
}