# Fumble

Functional wrapper around plain old `Sqlite` to simplify data access when talking to SQLite databases.

## Available Packages:

| Library  | Version |
| ------------- | ------------- |
| Fumble  | [![nuget - Fumble](https://img.shields.io/nuget/v/Fumble.svg?colorB=green)](hhttps://www.nuget.org/packages/Fumble/) |


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
    |> Sqlite.connect
    |> Sqlite.query "SELECT * FROM dbo.[Users]"
    |> Sqlite.execute (fun read ->
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
    |> Sqlite.connect
    |> Sqlite.query "SELECT * FROM dbo.[users]"
    |> Sqlite.execute(fun read ->
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
    |> Sqlite.connect
    |> Sqlite.query "select * from dbo.[users]"
    |> Sqlite.execute (fun read ->
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
    |> Sqlite.connect
    |> Sqlite.query "SELECT name FROM dbo.[Products] where category = @category"
    |> Sqlite.parameters [ "@category", Sqlite.string category ]
    |> Sqlite.execute (fun read -> read.string "name")
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
            |> Sqlite.connect
            |> Sqlite.storedProcedure "user_exists"
            |> Sqlite.parameters [ "@username", Sqlite.string username ]
            |> Sqlite.execute (fun read -> read.bool 0)
            |> function
                | Ok [ result ] -> Ok result
                | Error error -> Error error
                | unexpected -> failwithf "Expected result %A"  unexpected
    }
```
### Executing a stored procedure with table-valued parameters
```fs
open Fumble
open System.Data

// get the connection from the environment
let connectionString() = Env.getVar "app_db"

let executeMyStoredProcedure () : Async<int> =
    // create a table-valued parameter
    let customSqlTypeName = "MyCustomSqlTypeName"
    let dataTable = new DataTable()
    dataTable.Columns.Add "FirstName" |> ignore
    dataTable.Columns.Add "LastName"  |> ignore
    // add rows to the table parameter
    dataTable.Rows.Add("John", "Doe") |> ignore
    dataTable.Rows.Add("Jane", "Doe") |> ignore
    dataTable.Rows.Add("Fred", "Doe") |> ignore

    connectionString()
    |> Sqlite.connect
    |> Sqlite.storedProcedure "my_stored_proc"
    |> Sqlite.parameters
        [ "@foo", Sqlite.int 1
          "@people", Sqlite.table (customSqlTypeName, dataTable) ]
    |> Sqlite.executeNonQueryAsync
```

## Running Tests locally

You only need a working local Sqlite. The tests will create databases when required and dispose of them at the end of the each test

