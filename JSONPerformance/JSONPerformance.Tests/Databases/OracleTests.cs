using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(JSONPerformance.Databases.Oracle))]
public class OracleTests
{
    [Fact]
    public async Task ThrowExceptionIfConnectionStringIsNotCorrectSqlServerFormat()
    {
        // Arrange
        string connectionString = "testConnectionString";
        
        // Act
        var action = () => new JSONPerformance.Databases.Oracle(connectionString);
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}