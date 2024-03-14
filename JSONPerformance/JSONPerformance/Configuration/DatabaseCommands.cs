using System.ComponentModel;
using CommandDotNet;

namespace JSONPerformance.Configuration;

[Command("Database")]
[Subcommand]
public class DatabaseCommands
{
    public async Task Truncate(PossibleDatabases databases, string connectionString, string tableName)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        await database.Truncate(tableName);
        
        Console.WriteLine($"Data collection - {tableName} was successfully truncated in database - {databases.ToString()}");
    }
    
    public async Task Seed(PossibleDatabases databases, string connectionString, string pathToData)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        var data = await File.ReadAllLinesAsync(pathToData);
        await database.SeedDatabase(data);
        
        Console.WriteLine($"Database - {databases.ToString()} - was successfully seeded.");
    }

    public async Task TruncateAndSeed(PossibleDatabases databases, string connectionString, string tableName, string pathToData)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        await database.Truncate(tableName);
        Console.WriteLine($"Data collection - {tableName} was successfully truncated in database - {databases.ToString()}");
        
        var data = await File.ReadAllLinesAsync(pathToData);
        await database.SeedDatabase(data);
        
        Console.WriteLine($"Database - {databases.ToString()} - was successfully seeded.");
    }
}