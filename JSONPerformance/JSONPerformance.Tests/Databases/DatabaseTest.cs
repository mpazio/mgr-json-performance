using JetBrains.Annotations;
using JSONPerformance.Databases;
using Moq;
using Xunit;

namespace JSONPerformance.Tests.Databases;

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