using System.Diagnostics;

namespace JSONPerformance.Utils;

public static class PerformanceTimer
{
    public static async Task<TimeSpan> Measure(Func<string, string[], Task> function, string query, string[] parameters)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        await function(query, parameters);
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        return ts;
    }
}