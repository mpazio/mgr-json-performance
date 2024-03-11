using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JSONPerformance.Utils;
using Xunit;

namespace JSONPerformance.Tests.Utils;

[TestSubject(typeof(PerformanceTimer))]
public class PerformanceTimerTest
{

    [Fact]
    public async Task ReturnTrueIfTimeMeasureIsWithinErrorMargin()
    {
        // Arrange
        var errorMargin = TimeSpan.FromMilliseconds(100);
        var timeSpanWithErrorMargin = TimeSpan.FromSeconds(1).Add(errorMargin);
        
        // Act
        var res = await PerformanceTimer.Measure(WaitOneSecond, "someMessage");

        // Assert
        Assert.True(res < timeSpanWithErrorMargin);
    }

    private static Task WaitOneSecond(string parameter)
    {
        Thread.Sleep(1000);
        return Task.CompletedTask;
    }
}