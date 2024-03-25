# mgr-json-performance

CLI application to measure performance and integrity of JSON queries.
Data can be generated in seperate project - [mgr-data-generator](https://github.com/mpazio/mgr-data-generator).
Apart from measuring performance it supports truncating and seeding data.

## Table of contents

- [Supported databases](#supported-databases)
- [How to use](#how-to-use)
- [Publish](#publish)
- [Example usage](#example-usage)

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
