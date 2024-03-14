using CommandDotNet;
using JSONPerformance.Utils;

namespace JSONPerformance.Configuration;

[Command("Performance")]
[Subcommand]
public class PerformanceCommands
{
    public async Task TimeQuery(PossibleDatabases databases, string connectionString, string query)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        await database.Connect();
        for (int i = 0; i < 10; i++)
        {
            var time = await PerformanceTimer.Measure(database.ExecuteQuery, query);
            Console.WriteLine(time); 
        }
    }
}