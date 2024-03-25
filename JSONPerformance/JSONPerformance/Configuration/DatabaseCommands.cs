using System.ComponentModel;
using CommandDotNet;

namespace JSONPerformance.Configuration;

[Command("Database")]
[Subcommand]
public class DatabaseCommands
{
    public async Task Truncate(
        [Operand("database", Description = "Name of the database where the data will be truncated")]
        PossibleDatabases databases, 
        [Operand("connectionString", Description = "Valid connection string to selected database")]
        string connectionString, 
        [Operand("tableName", Description = "Name of the table/cluster/persistence storage from which data will be truncated")]
        string tableName, 
        [Option('p', "param", Description = "Additional parameters for specific database cases")] params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        try
        {
            await database.Connect();
            if (!await database.IsConnected())
                throw new Exception("Not connected to database");
            await database.Truncate(tableName, additionalParameters);
        
            Console.WriteLine($"Data collection - {tableName} was successfully truncated in database - {databases.ToString()}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Truncating failed!");
            throw;
        }
    }
    
    
    public async Task Seed(
        [Operand("database", Description = "Name of the database where the data will be seeded")]
        PossibleDatabases databases, 
        [Operand("connectionString", Description = "Valid connection string to selected database")]
        string connectionString, 
        [Operand("pathToData", Description = "Local path to data file with inserts or JSON data that will be used for seeding")]
        string pathToData, 
        [Option('p', "param", Description = "Additional parameters for specific database cases")] params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        try
        {
            await database.Connect();
            if (!await database.IsConnected())
                throw new Exception("Not connected to database");
            var data = await File.ReadAllLinesAsync(pathToData);
            await database.SeedDatabase(data, additionalParameters);

            Console.WriteLine($"Database - {databases.ToString()} - was successfully seeded.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Seeding failed!");
            throw;
        }
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