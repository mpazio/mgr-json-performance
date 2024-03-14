using JSONPerformance.Databases;

namespace JSONPerformance.Configuration;

public static class CommandsHelpers
{
    public static Database GetDatabases(PossibleDatabases databases, string connectionString)
    {
        switch (databases)
        {
            case PossibleDatabases.Couchbase:
                return new Databases.Couchbase(connectionString);
            case PossibleDatabases.CouchDb:
                return new CouchDb(connectionString);
            case PossibleDatabases.MongoDb:
                return new MongoDb(connectionString);
            case PossibleDatabases.Oracle:
                return new Databases.Oracle(connectionString);
            case PossibleDatabases.Postgres:
                return new Postgres(connectionString);
            case PossibleDatabases.Redis:
                return new Redis(connectionString);
            case PossibleDatabases.SqlServer:
                return new SqlServer(connectionString);
            default:
                return new Databases.Couchbase(connectionString);
        }
    }
}