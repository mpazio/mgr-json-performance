using CommandDotNet;

namespace JSONPerformance.Configuration;

[Command("Integrity")]
[Subcommand]
public class IntegrityCommands
{
    public async Task Run(
        PossibleDatabases databases, 
        string connectionString, 
        string query,
        [Option('f', "filepath", Description = "Path where result file will be created")] 
        string path = ".",
        [Option('p', "param", Description = "Additional parameters for specific database cases")] 
        params string[]? additionalParameters
    )
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        try
        {
            await database.Connect();
            List<TimeSpan> timespanList = new List<TimeSpan>();
            var res = await database.ExecuteQueryAndReturnStringResult(query, additionalParameters);
            
            await File.WriteAllTextAsync(path+"/integrityResult.json", res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}