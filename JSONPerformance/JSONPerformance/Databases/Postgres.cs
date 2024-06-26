﻿using System.Data;
using Npgsql;

namespace JSONPerformance.Databases;

public class Postgres : Database
{
    private readonly NpgsqlDataSource _dataSource;
    public NpgsqlConnection? Connection { get; private set; }

    public Postgres(string connectionString) : base(connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        _dataSource = dataSourceBuilder.Build();
    }
    
    public override async Task Connect()
    {
        Connection = await _dataSource.OpenConnectionAsync();
    }

    public override Task<bool> IsConnected()
    {
        if (Connection is null) return Task.FromResult(false);
        switch (Connection.State)
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
        await using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = Connection;
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
        await using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = Connection;
            cmd.CommandText = $"TRUNCATE TABLE {tableName}";
            
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public override async Task ExecuteQuery(string query, params string[]? parameters)
    {
        if (!await IsConnected()) return;
        await using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = Connection;
            cmd.CommandText = query;

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public override async Task<string> ExecuteQueryAndReturnStringResult(string query, params string[]? parameters)
    {
        if (!await IsConnected()) return "";

        string res = "";
        await using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = Connection;
            cmd.CommandText = query;

            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                res += reader.GetValue(0) + "\n";
            }
        }

        return res;
    }
}