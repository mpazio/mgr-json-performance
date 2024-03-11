using JetBrains.Annotations;
using JSONPerformance;
using Moq;
using Xunit;

namespace JSONPerformance.Tests;

[TestSubject(typeof(Database))]
public class DatabaseTest
{

    [Fact]
    public void CheckIfConnectionStringIsSetProperly()
    {
        // Arrange
        const string connectionString = "testConnectionString";
        
        // Act
        var mock = new Mock<Database>(connectionString);

        // Assert
        Assert.Equal(connectionString, mock.Object.ConnectionString);
    }
}