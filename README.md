# mgr-json-performance

CLI application to measure performance and integrity of JSON queries.
Data can be generated in seperate project - [mgr-data-generator](https://github.com/mpazio/mgr-data-generator).
Apart from measuring performance it supports truncating and seeding data.

## Table of contents

- [Supported databases](#supported-databases)
- [How to use](#how-to-use)
- [Publish](#publish)
- [Example usage](#example-usage)
  - [Database - truncate](#database-truncate)
  - [Database - seed](#database-seed)
  - [Database - truncate and seed](#database-truncate-and-seed)
  - [Database - time query](#database-time-query)

<a name='supported-databases'></a>

## Supported databases

- Postgres
- SqlServer
- Oracle
- MongoDb
- Redis
- Couchbase
- CouchDb

<a name='how-to-use'></a>

## How to use

After publishing working version of the application, all operations are accessed throught commands in terminal.
Every command is documented and commands cannot be executed without proper arguments.
Help is displayed after wrongly executing commands or after using `--help` flag.

Below is example of the interface:

```console
Usage: .\JSONPerformance.exe [command]

Commands:

  Database
  Integrity
  Performance
```

<a name='publish'></a>

## Publish

Before use, it is necessary to deploy / publish the application, as not all parameters will work with the basic `dotnet run` application.
To do this, you can use the following command:

```console
dotnet publish --output .\dist\ --runtime win-x64 -p:PublishSingleFile=true --self-contained false
```

Latest version of the application can be found in Releases in the repository.

<a name='example-usage'></a>

## Example usage

<a name='database-truncate'></a>

### Database - truncate

Usage:

```
Usage: JSONPerformance.exe Database Truncate [options] <database> <connectionString> <tableName>

Arguments:

  database          <POSSIBLEDATABASES>
  Name of the database where the data will be truncated
  Allowed values: Couchbase, CouchDb, MongoDb, Oracle, Postgres, Redis, SqlServer

  connectionString  <TEXT>
  Valid connection string to selected database

  tableName         <TEXT>
  Name of the table/cluster/persistence storage from which data will be truncated

Options:

  -p | --param (Multiple)  <TEXT>
  Additional parameters for specific database cases
```

Example commands for every supported database:

- Postgres

  ```console
  .\JSONPerformance.exe Database Truncate Postgres "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres" jsondata
  ```

- SqlServer

  ```console
  .\JSONPerformance.exe Database Truncate SqlServer "Data Source=localhost;User ID=SA;Password=yourStrong(!)Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Initial Catalog=mgr" jsondata
  ```

- Oracle

  ```console
  .\JSONPerformance.exe Database Truncate Oracle "user id=mgr; password=admin; data source=localhost:1521/XEPDB1" jsondata
  ```

- MongoDb

  ```console
  .\JSONPerformance.exe Database Truncate MongoDb "mongodb://root:example@localhost:27017/" jsondata
  ```

- Redis

  ```console
  .\JSONPerformance.exe Database Truncate Redis localhost:7000 user
  ```

- Couchbase
  ```console
  .\JSONPerformance.exe Database Truncate Couchbase "couchbase://localhost" data
  ```
- CouchDb
  Needs special parameters: `login` and `password`
  ```console
  .\JSONPerformance.exe Database Truncate CouchDb "http://127.0.0.1:5984" test -p admin -p password
  ```
  <a name='database-seed'></a>

### Database - seed

Usage:

```
Usage: JSONPerformance.exe Database Seed [options] <database> <connectionString> <pathToData>

Arguments:

  database          <POSSIBLEDATABASES>
  Name of the database where the data will be seeded
  Allowed values: Couchbase, CouchDb, MongoDb, Oracle, Postgres, Redis, SqlServer

  connectionString  <TEXT>
  Valid connection string to selected database

  pathToData        <TEXT>
  Local path to data file with inserts or JSON data that will be used for seeding

Options:

  -p | --param (Multiple)  <TEXT>
  Additional parameters for specific database cases
```

Example commands for every supported database:

- Postgres

  ```console
  .\JSONPerformance.exe Database Seed Postgres "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres" "../jsondata/postgresinserts.txt"
  ```

- SqlServer

  ```console
  .\JSONPerformance.exe Database Seed SqlServer "Data Source=localhost;User ID=SA;Password=yourStrong(!)Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Initial Catalog=mgr" "../jsondata/sqlserverinserts.txt"
  ```

- Oracle

  ```console
  .\JSONPerformance.exe Database Seed Oracle "user id=mgr; password=admin; data source=localhost:1521/XEPDB1" "../jsondata/oracleinserts.txt"
  ```

- MongoDb

  ```console
  .\JSONPerformance.exe Database Seed MongoDb "mongodb://root:example@localhost:27017/" "../jsondata/genericinserts.txt"
  ```

- Redis
  Needs special parameters: `schema` and `path`

  ```console
  .\JSONPerformance.exe Database Seed Redis localhost:7000 "../jsondata/redisinserts.txt" -p "user" -p "$"
  ```

- Couchbase
  ```console
  .\JSONPerformance.exe Database Seed Couchbase "couchbase://localhost" "../jsondata/genericinserts.txt"
  ```
- CouchDb
  Needs special parameters: `login` and `password`
  ```console
  .\JSONPerformance.exe Database Seed CouchDb "http://127.0.0.1:5984" "../jsondata/genericinserts.txt" -p admin -p password
  ```

<a name='database-truncate-and-seed'></a>

### Database - truncate and seed

Usage:

```
Usage: JSONPerformance.exe Database TruncateAndSeed [options] <database> <connectionString> <tableName> <pathToData>

Arguments:

  database          <POSSIBLEDATABASES>
  Name of the database where the data will be seeded
  Allowed values: Couchbase, CouchDb, MongoDb, Oracle, Postgres, Redis, SqlServer

  connectionString  <TEXT>
  Valid connection string to selected database

  tableName         <TEXT>
  Name of the table/cluster/persistence storage from which data will be truncated

  pathToData        <TEXT>
  Local path to data file with inserts or JSON data that will be used for seeding

Options:

  -p | --param (Multiple)  <TEXT>
  Additional parameters for specific database cases
```

Example commands for every supported database:

- Postgres

  ```console
  .\JSONPerformance.exe Database TruncateAndSeed Postgres "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres" jsondata "../jsondata/postgresinserts.txt"
  ```

- SqlServer

  ```console
  .\JSONPerformance.exe Database TruncateAndSeed SqlServer "Data Source=localhost;User ID=SA;Password=yourStrong(!)Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Initial Catalog=mgr" jsondata "../jsondata/sqlserverinserts.txt"
  ```

- Oracle

  ```console
  .\JSONPerformance.exe Database TruncateAndSeed Oracle "user id=mgr; password=admin; data source=localhost:1521/XEPDB1" jsondata "../jsondata/oracleinserts.txt"
  ```

- MongoDb

  ```console
   .\JSONPerformance.exe Database TruncateAndSeed MongoDb "mongodb://root:example@localhost:27017/" jsondata "../jsondata/genericinserts.txt"
  ```

- Redis
  Needs special parameters: `schema` and `path`

  ```console
  .\JSONPerformance.exe Database TruncateAndSeed Redis localhost:7000 jsondata "../jsondata/redisinserts.txt" -p "user" -p "$"
  ```

- Couchbase
  ```console
  .\JSONPerformance.exe Database TruncateAndSeed Couchbase "couchbase://localhost" data "../jsondata/genericinserts.txt"
  ```
- CouchDb
  Needs special parameters: `login` and `password`
  ```console
  .\JSONPerformance.exe Database TruncateAndSeed CouchDb "http://127.0.0.1:5984" test "../jsondata/genericinserts.txt" -p admin -p password
  ```

<a name='database-time-query'></a>

### Database - time query

Usage:

```
Usage: JSONPerformance.exe Performance TimeQuery [options] <databases> <connectionString> <query>

Arguments:

  databases         <POSSIBLEDATABASES>
  Allowed values: Couchbase, CouchDb, MongoDb, Oracle, Postgres, Redis, SqlServer

  connectionString  <TEXT>

  query             <TEXT>

Options:

  -p | --path   <TEXT>    [.]
  Path where result file will be created

  -c | --count  <NUMBER>  [10]
  Number of runs that will be executed (Default is 10)
```

This command will create `result.json` file:

```
{
  "Average": {
    "Hours": 0,
    "Minutes": 0,
    "Seconds": 0,
    "Milliseconds": 12,
    "TimeSpan": "00:00:00.0124901"
  },
  "Times": [
    {
      "Hours": 0,
      "Minutes": 0,
      "Seconds": 0,
      "Milliseconds": 107,
      "TimeSpan": "00:00:00.1072575"
    },
    .
    . rest of the runs
    .
    {
      "Hours": 0,
      "Minutes": 0,
      "Seconds": 0,
      "Milliseconds": 1,
      "TimeSpan": "00:00:00.0016773"
    }
  ]
}
```

Example commands for every supported database:

- Postgres

  ```console
  .\JSONPerformance.exe Performance TimeQuery Postgres "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=postgres" "SELECT * FROM jsondata"
  ```

- SqlServer

  ```console
  .\JSONPerformance.exe Performance TimeQuery SqlServer "Data Source=localhost;User ID=SA;Password=yourStrong(!)Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Initial Catalog=mgr" "SELECT * FROM jsondata"
  ```

- Oracle

  ```console
  .\JSONPerformance.exe Performance TimeQuery Oracle "user id=mgr; password=admin; data source=localhost:1521/XEPDB1" "SELECT * FROM jsondata"
  ```

- MongoDb

  ```console
  .\JSONPerformance.exe Performance TimeQuery MongoDb "mongodb://root:example@localhost:27017/" "{\`"find\`": \`"jsondata\`"}"
  ```

- Redis

  ```console
  .\JSONPerformance.exe Performance TimeQuery Redis localhost:7000 "FT.SEARCH userIndex '@firstName:(Arvid)'"
  ```

- Couchbase
  ```console
  .\JSONPerformance.exe Performance TimeQuery Couchbase "couchbase://localhost" "SELECT * FROM `data` WHERE data.firstName = \`"Willie\`""
  ```
- CouchDb
  ```console
  .\JSONPerformance.exe Performance TimeQuery CouchDb "http://127.0.0.1:5984" "{\`"selector\`":{\`"_id\`":{\`"`$gt\`":null}}}"
  ```
