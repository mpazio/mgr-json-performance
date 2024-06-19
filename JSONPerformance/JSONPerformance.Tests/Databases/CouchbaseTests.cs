using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(JSONPerformance.Databases.Couchbase))]
public class CouchbaseTests
{
    [Fact]
    public async Task ThrowExceptionIfConnectionStringIsNotCorrectSqlServerFormat()
    {
        // Arrange
        string connectionString = "testConnectionString";
        var db = new JSONPerformance.Databases.Couchbase(connectionString);
        
        
        // Act
        var action = new Func<Task>(async () => await db.Connect());
        
        // Assert
        await Assert.ThrowsAsync<StackExchange.Redis.RedisConnectionException>(action);
    }
}