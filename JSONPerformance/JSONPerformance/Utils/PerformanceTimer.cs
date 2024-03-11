using System.Diagnostics;

namespace JSONPerformance.Utils;

public static class PerformanceTimer
{
    public static async Task<TimeSpan> Measure(Func<string, Task> function, string parameter)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        await function(parameter);
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        return ts;
    }
}