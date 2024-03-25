using System.ComponentModel;
using CommandDotNet;

namespace JSONPerformance.Configuration;

[Command("Database")]
[Subcommand]
public class DatabaseCommands
{
    public async Task Truncate(PossibleDatabases databases, string connectionString, string tableName, [Option('p', "param")] params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        if (!await database.IsConnected())
            throw new Exception("Not connected to database");
        await database.Truncate(tableName, additionalParameters);
        
        Console.WriteLine($"Data collection - {tableName} was successfully truncated in database - {databases.ToString()}");
    }
    
    
    public async Task Seed(PossibleDatabases databases, string connectionString, string pathToData, [Option('p', "param")] params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        if (!await database.IsConnected())
            throw new Exception("Not connected to database");
        var data = await File.ReadAllLinesAsync(pathToData);
        await database.SeedDatabase(data, additionalParameters);
        
        Console.WriteLine($"Database - {databases.ToString()} - was successfully seeded.");
    }

    public async Task TruncateAndSeed(PossibleDatabases databases, string connectionString, string tableName, string pathToData, [Option('p', "param")] params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        if (!await database.IsConnected())
            throw new Exception("Not connected to database");
        await database.Truncate(tableName);
        Console.WriteLine($"Data collection - {tableName} was successfully truncated in database - {databases.ToString()}");
        
        var data = await File.ReadAllLinesAsync(pathToData);
        await database.SeedDatabase(data, additionalParameters);
        
        Console.WriteLine($"Database - {databases.ToString()} - was successfully seeded.");
    }
}