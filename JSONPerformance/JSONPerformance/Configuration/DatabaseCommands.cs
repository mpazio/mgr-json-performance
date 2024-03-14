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
    }
    
    public void Seed(PossibleDatabases databases, string connectionString, string pathToData)
    {
        
    }
}