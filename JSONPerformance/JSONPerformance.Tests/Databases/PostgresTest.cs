using System;
using JetBrains.Annotations;
using JSONPerformance.Databases;
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

}