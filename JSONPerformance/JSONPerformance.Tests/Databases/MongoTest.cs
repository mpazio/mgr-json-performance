using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JSONPerformance.Databases;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(Mongo))]
public class MongoTest
{

    [Fact]
    public void CheckIfMongoClientIsCreatedDuringConstruction()
    {
        // Arrange
        var mongo = new Mongo("mongodb://localhost:27017");

        // Assert
        Assert.IsType<MongoClient>(mongo.Client);
    }
    
    [Fact]
    public async Task ConnectMethodSetsNewMongoClient()
    {
        // Arrange
        var connectionString = "mongodb://localhost:27017";
        var mockMongoClient = new Mock<IMongoClient>();
        var mongo = new Mongo(connectionString);
        mongo.Client = mockMongoClient.Object;

        // Act
        await mongo.Connect();

        // Assert
        Assert.NotNull(mongo.Client);
        Assert.IsType<MongoClient>(mongo.Client);
    }
    
    [Fact]
    public async Task IsConnectedReturnsTrueWhenConnectionSucceeds()
    {
        // Arrange
        var connectionString = "mongodb://localhost:27017";
        var mockMongoClient = new Mock<IMongoClient>();
        var mockAsyncCursor = new Mock<IAsyncCursor<string>>();
        mockAsyncCursor.Setup(m => m.MoveNextAsync(default)).ReturnsAsync(true);
        mockAsyncCursor.Setup(m => m.Current).Returns(new List<string> { "db1", "db2" });

        mockMongoClient.Setup(m => m.ListDatabaseNamesAsync(default)).ReturnsAsync(mockAsyncCursor.Object);

        var mongo = new Mongo(connectionString);
        mongo.Client = mockMongoClient.Object;

        // Act
        var isConnected = await mongo.IsConnected();

        // Assert
        Assert.True(isConnected);
    }
    
    [Fact]
    public async Task IsConnectedReturnsFalseWhenConnectionFails()
    {
        // Arrange
        var connectionString = "mongodb://localhost:27017";
        var mockMongoClient = new Mock<IMongoClient>();
        mockMongoClient.Setup(m => m.ListDatabaseNamesAsync(default)).ThrowsAsync(null!);

        var mongo = new Mongo(connectionString);
        mongo.Client = mockMongoClient.Object;

        // Act
        var isConnected = await mongo.IsConnected();

        // Assert
        Assert.False(isConnected);
    }
}