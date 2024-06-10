using System.Text.Json;
using CommandDotNet;
using JSONPerformance.Utils;

namespace JSONPerformance.Configuration;

[Command("Performance")]
[Subcommand]
public class PerformanceCommands
{
    public async Task TimeQuery(
        PossibleDatabases databases, 
        string connectionString, 
        string query,
        [Option('p', "path", Description = "Path where result file will be created")] 
        string path = ".",
        [Option('c', "count", Description = "Number of runs that will be executed (Default is 10)")] 
        int numberOfRuns = 10,
        [Option('a', "param", Description = "Additional parameters for specific database cases")] 
        params string[]? additionalParameters)
    {
        var database = CommandsHelpers.GetDatabases(databases, connectionString);
        try
        {
            await database.Connect();
            List<TimeSpan> timespanList = new List<TimeSpan>();
            for (int i = 0; i < numberOfRuns; i++)
            {
                var time = await PerformanceTimer.Measure(database.ExecuteQuery, query, additionalParameters ?? new string[] {});
                timespanList.Add(time);
            }

            var serialized = GetSerializedData(timespanList);
            await File.WriteAllTextAsync(path+"/result.json", serialized);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string GetSerializedData(List<TimeSpan> timespanList)
    {
        List<Time> timeList = new List<Time>();
        foreach (var timeSpan in timespanList)
        {
            timeList.Add(new Time()
            {
                Hours = timeSpan.Hours,
                Minutes = timeSpan.Minutes,
                Seconds = timeSpan.Seconds,
                Milliseconds = timeSpan.Milliseconds,
                TimeSpan = timeSpan
            });
        }

        var avg = TimeSpan.FromSeconds(timespanList.Select(s => s.TotalSeconds).Average());
        var averageTime = new Time()
        {
            Hours = avg.Hours,
            Minutes = avg.Minutes,
            Seconds = avg.Seconds,
            Milliseconds = avg.Milliseconds,
            TimeSpan = avg
        };
        
        return JsonSerializer.Serialize(new
        {
            Average = averageTime,
            Times = timeList
        }, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        });
    }
}

public class Time
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public int Milliseconds { get; set; }
    public TimeSpan TimeSpan { get; set; }
}