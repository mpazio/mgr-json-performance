using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JSONPerformance.Databases;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(JSONPerformance.Databases.Oracle))]
public class RedisTests
{
    [Fact]
    public async Task ThrowExceptionIfConnectionStringIsNotCorrectSqlServerFormat()
    {
        // Arrange
        string connectionString = "testConnectionString";
        var db = new Redis(connectionString);
        
        
        // Act
        var action = new Func<Task>(async () => await db.Connect());
        
        // Assert
        await Assert.ThrowsAsync<StackExchange.Redis.RedisConnectionException>(action);
    }
}