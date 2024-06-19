using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JSONPerformance.Databases;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(JSONPerformance.Databases.SqlServer))]
public class SqlServerTests
{
    [Fact]
    public async Task ThrowExceptionIfConnectionStringIsNotCorrectSqlServerFormat()
    {
        // Arrange
        string connectionString = "testConnectionString";
        var db = new SqlServer(connectionString);
        
        
        // Act
        var action = new Func<Task>(async () => await db.Connect());
        
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);
    }
}