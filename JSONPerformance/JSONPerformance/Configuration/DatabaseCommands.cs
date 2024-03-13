using System.ComponentModel;
using CommandDotNet;

namespace JSONPerformance.Configuration;

[Command("Database")]
[Subcommand]
public class DatabaseCommands
{
    public void Truncate(PossibleDatabases databases)
    {
        
    }
    
    public void Seed(PossibleDatabases databases, string pathToData)
    {
        
    }
}