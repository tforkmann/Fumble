# Fumble

Functional wrapper around plain old `Sqlite` to simplify data access when talking to SQLite databases.

## Available Packages:

| Library  | Version |
| ------------- | ------------- |
| Fumble  | [![nuget - Fumble](https://img.shields.io/nuget/v/Fumble.svg?colorB=green)](https://www.nuget.org/packages/Fumble/) |


## Install
```bash
# nuget client
dotnet add package Fumble

# or using paket
.paket/paket.exe add Fumble --project path/to/project.fsproj
```

## Query a table
```fs
open Fumble

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

type User = { Id: int; Username: string }

let getUsers() : Result<User list, exn> =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT * FROM dbo.[Users]"
    |> Sql.execute (fun read ->
        {
            Id = read.int "user_id"
            Username = read.string "username"
        })
```

## Handle null values from table columns:
```fs
open Fumble

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

type User = { Id: int; Username: string; LastModified : Option<DateTime> }

let getUsers() : Result<User list, exn> =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT * FROM dbo.[users]"
    |> Sql.execute(fun read ->
        {
            Id = read.int "user_id"
            Username = read.string "username"
            // Notice here using `orNone` reader variants
            LastModified = read.dateTimeOrNone "last_modified"
        })
```
## Providing default values for null columns:
```fs
open Fumble

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

type User = { Id: int; Username: string; Biography : string }

let getUsers() : Result<User list, exn> =
    connectionString()
    |> Sql.connect
    |> Sql.query "select * from dbo.[users]"
    |> Sql.execute (fun read ->
        {
            Id = read.int "user_id";
            Username = read.string "username"
            Biography = defaultArg (read.stringOrNone "bio") ""
        })
```
## Execute a parameterized query
```fs
open Fumble

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

// get product names by category
let productsByCategory (category: string) : Result<string list, exn> =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT name FROM dbo.[Products] where category = @category"
    |> Sql.parameters [ "@category", Sql.string category ]
    |> Sql.execute (fun read -> read.string "name")
```

## Supported Data Types

### Basic Types
| F# Type | Parameter | Reader |
|---------|-----------|--------|
| `int` | `Sql.int` | `read.int` |
| `int16` | `Sql.int16` | `read.int16` |
| `int64` | `Sql.int64` | `read.int64` |
| `string` | `Sql.string` | `read.string` |
| `bool` | `Sql.bool` | `read.bool` |
| `decimal` | `Sql.decimal` | `read.decimal` |
| `double` | `Sql.double` | `read.double` |
| `float` | `Sql.double` | `read.float` |
| `float32` | `Sql.double` | `read.float32` |
| `Guid` | `Sql.uniqueidentifier` | `read.uniqueidentifier` |
| `byte[]` | `Sql.bytes` | `read.bytes` |
| `DateTime` | `Sql.dateTime` | `read.dateTime` |
| `DateTimeOffset` | `Sql.dateTimeOffset` | `read.dateTimeOffset` |

### New in v2.0
| F# Type | Parameter | Reader | Storage |
|---------|-----------|--------|---------|
| `TimeSpan` | `Sql.timeSpan` | `read.timeSpan` | INTEGER (ticks) |
| `DateOnly` | `Sql.dateOnly` | `read.dateOnly` | TEXT (yyyy-MM-dd) |
| `TimeOnly` | `Sql.timeOnly` | `read.timeOnly` | INTEGER (ticks) |
| `byte` | `Sql.byte` | `read.tinyint` | INTEGER |
| `uint32` | `Sql.uint32` | `read.uint32` | INTEGER |
| `uint64` | `Sql.uint64` | `read.uint64` | INTEGER |

All types have `OrNone` variants for nullable columns (e.g., `Sql.intOrNone`, `read.intOrNone`).

## New in v2.0: Additional Features

### Execute Scalar Queries
```fs
// Get a single value
let count =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT COUNT(*) FROM Users"
    |> Sql.executeScalar<int64>
    // Returns: Result<int64 option, exn>

// Check if data exists
let exists =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT 1 FROM Users WHERE Username = @name"
    |> Sql.parameters [ "@name", Sql.string "john" ]
    |> Sql.executeExists
    // Returns: Result<bool, exn>

// Get count directly
let userCount =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT COUNT(*) FROM Users"
    |> Sql.executeCount
    // Returns: Result<int64, exn>
```

### Pagination
```fs
// Using take (LIMIT)
let firstTen =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT * FROM Users ORDER BY Id"
    |> Sql.take 10
    |> Sql.execute (fun read -> read.string "Username")

// Using paginate (LIMIT + OFFSET)
let page2 =
    connectionString()
    |> Sql.connect
    |> Sql.query "SELECT * FROM Users ORDER BY Id"
    |> Sql.paginate 2 10  // page 2, 10 items per page
    |> Sql.execute (fun read -> read.string "Username")
```

### In-Memory Database
```fs
// Create an in-memory database
Sql.connectInMemory ()
|> Sql.query "CREATE TABLE Test (Id INTEGER PRIMARY KEY, Name TEXT)"
|> Sql.executeNonQuery

// Shared in-memory database (accessible from multiple connections)
Sql.connectInMemoryShared "mydb"
|> Sql.query "SELECT * FROM Test"
|> Sql.execute (fun read -> read.string "Name")
```

### SQLite Pragmas and Introspection
```fs
// Get SQLite version
let version =
    connectionString()
    |> Sql.connect
    |> Sql.sqliteVersion
    // Returns: Result<string option, exn>

// List all tables
let tables =
    connectionString()
    |> Sql.connect
    |> Sql.listTables
    // Returns: Result<string list, exn>

// Get table schema info
let columns =
    connectionString()
    |> Sql.connect
    |> Sql.tableInfo "Users"
    // Returns column info with Name, Type, NotNull, DefaultValue, IsPrimaryKey

// Enable WAL mode for better concurrency
connectionString()
|> Sql.connect
|> Sql.enableWalMode

// Enable foreign keys
connectionString()
|> Sql.connect
|> Sql.enableForeignKeys

// Database maintenance
connectionString()
|> Sql.connect
|> Sql.vacuum  // Rebuild database file

connectionString()
|> Sql.connect
|> Sql.analyze  // Update statistics
```

### Bulk Operations
```fs
// Bulk delete
connectionString()
|> Sql.connect
|> Sql.bulkDelete "Users" "Id" [1; 2; 3]
// Deletes users with Id 1, 2, or 3

// Upsert (INSERT OR REPLACE)
type User = { Id: int; Name: string; Email: string }

let users = [
    { Id = 1; Name = "John"; Email = "john@example.com" }
    { Id = 2; Name = "Jane"; Email = "jane@example.com" }
]

connectionString()
|> Sql.connect
|> Sql.upsert "Users" users
// Inserts or replaces based on primary key
```

### Auto-generate CREATE TABLE
```fs
type User = {
    Id: int
    Username: string
    Email: string option
    CreatedAt: DateTime
}

// Creates: CREATE TABLE IF NOT EXISTS [Users]
//    ([Id] INTEGER,
//     [Username] TEXT,
//     [Email] TEXT NULL,
//     [CreatedAt] TEXT)
connectionString()
|> Sql.connect
|> Sql.commandCreate<User> "Users"
|> Sql.executeCommand
```

### Executing a stored procedure with parameters
```fs
open Fumble

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

// check whether a user exists or not
let userExists (username: string) : Async<Result<bool, exn>> =
    async {
        return!
            connectionString()
            |> Sql.connect
            |> Sql.storedProcedure "user_exists"
            |> Sql.parameters [ "@username", Sql.string username ]
            |> Sql.execute (fun read -> read.bool 0)
            |> function
                | Ok [ result ] -> Ok result
                | Error error -> Error error
                | unexpected -> failwithf "Expected result %A"  unexpected
    }
```

## Running Tests locally

You only need a working local Sqlite. The tests will create databases when required and dispose of them at the end of the each test.

```bash
dotnet run --project tests/Fumble.Tests/FumbleTests.fsproj
```
