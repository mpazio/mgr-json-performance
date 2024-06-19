using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JSONPerformance.Contracts;
using JSONPerformance.Databases;
using Moq;
using Xunit;

namespace JSONPerformance.Tests.Databases;

[TestSubject(typeof(JSONPerformance.Databases.Postgres))]
public class PostgresTest
{
    
    [Fact]
    public void ThrowExceptionIfConnectionStringIsNotCorrectPostgresFormat()
    {
        // Arrange
        string connectionString = "testConnectionString";
        
        // Act
        Action action = () => new Postgres(connectionString);
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void CheckIfDatabaseIsCorrectlyCreated()
    {
        // Arrange
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres";
        
        // Act
        var postgres = new Postgres(connectionString);
        
        // Assert
        Assert.IsType<Postgres>(postgres);
    }

    [Fact]
    public async Task METHOD()
    {
        // Arrange
        Mock<IDbConnection> mockDataSource = new Mock<IDbConnection>();
        Postgres postgres = new Postgres("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres");
        mockDataSource.Setup(ds => ds.Open());

        // Act
        await postgres.Connect();

        // Assert
        Assert.NotNull(postgres.Connection);
    }
    
    [Fact]
    public async Task METHOD2()
    {
        // Arrange
        var mockDataSource = new Mock<IDbConnection>();
        var post = new Mock<Database>();
        post.Setup(e => e.Connect()).Returns(Task.CompletedTask);
        mockDataSource.Setup(ds => ds.Open()).Throws(new Exception("Connection failed"));

        var postgres = new Postgres("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres");
        
        // Act
        var action = new Func<Task>(async () => await postgres.Connect());
        
        // Assert
        await Assert.ThrowsAsync<Npgsql.NpgsqlException>(action);
        // var exception = await task;
        // Assert.NotNull(exception);
        // if (exception != null) Assert.Equal("Connection failed", exception.Message);
        // Assert.Null(postgres.Connection);
    }
}