module Docs.Pages.NewFeatures

open Feliz
open Docs.SharedView

[<ReactComponent>]
let NewFeaturesView () =
    React.fragment [
        Html.divClassed "description" [
            Html.text "New features in Fumble v2.0"
        ]

        // New Data Types
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "New Data Types" ]
        Html.divClassed "description" [
            Html.text "Support for modern .NET types:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// TimeSpan (stored as ticks)
Sql.timeSpan (TimeSpan.FromHours(2.5))
read.timeSpan "Duration"

// DateOnly (stored as yyyy-MM-dd)
Sql.dateOnly (DateOnly(2024, 1, 15))
read.dateOnly "BirthDate"

// TimeOnly (stored as ticks)
Sql.timeOnly (TimeOnly(14, 30, 0))
read.timeOnly "AlarmTime"

// Unsigned integers
Sql.uint32 42u
Sql.uint64 123456789UL
read.uint32 "Counter"
read.uint64 "BigCounter"

// Single byte
Sql.byte 255uy
read.tinyint "SmallValue" """
        ]

        // Execute Scalar
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "Execute Scalar Queries" ]
        Html.divClassed "description" [
            Html.text "Get single values directly:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// Get a single value
connectionString()
|> Sql.connect
|> Sql.query "SELECT COUNT(*) FROM Users"
|> Sql.executeScalar<int64>
// Returns: Result<int64 option, exn>

// Check if data exists
connectionString()
|> Sql.connect
|> Sql.query "SELECT 1 FROM Users WHERE Id = @id"
|> Sql.parameters [ "@id", Sql.int 1 ]
|> Sql.executeExists
// Returns: Result<bool, exn>

// Get count directly
connectionString()
|> Sql.connect
|> Sql.query "SELECT COUNT(*) FROM Users"
|> Sql.executeCount
// Returns: Result<int64, exn>"""
        ]

        // Pagination
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "Pagination" ]
        Html.divClassed "description" [
            Html.text "Easy pagination with LIMIT and OFFSET:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// Using take (LIMIT)
connectionString()
|> Sql.connect
|> Sql.query "SELECT * FROM Users ORDER BY Id"
|> Sql.take 10
|> Sql.execute (fun read -> read.string "Username")

// Using paginate (page, pageSize)
connectionString()
|> Sql.connect
|> Sql.query "SELECT * FROM Users ORDER BY Id"
|> Sql.paginate 2 10  // page 2, 10 items per page
|> Sql.execute (fun read -> read.string "Username")

// Using skip and take separately
connectionString()
|> Sql.connect
|> Sql.query "SELECT * FROM Users"
|> Sql.take 10
|> Sql.skip 20
|> Sql.execute (fun read -> read.string "Username")"""
        ]

        // In-Memory Database
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "In-Memory Database" ]
        Html.divClassed "description" [
            Html.text "Create in-memory databases for testing:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// Simple in-memory database
Sql.connectInMemory ()
|> Sql.query "CREATE TABLE Test (Id INTEGER, Name TEXT)"
|> Sql.executeNonQuery

// Shared in-memory database
// (accessible from multiple connections)
Sql.connectInMemoryShared "testdb"
|> Sql.query "SELECT * FROM Test"
|> Sql.execute (fun read -> read.string "Name")"""
        ]

        // SQLite Pragmas
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "SQLite Pragmas & Introspection" ]
        Html.divClassed "description" [
            Html.text "Access SQLite-specific features:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// Get SQLite version
connectionString()
|> Sql.connect
|> Sql.sqliteVersion

// List all tables
connectionString()
|> Sql.connect
|> Sql.listTables

// Get table schema
connectionString()
|> Sql.connect
|> Sql.tableInfo "Users"
// Returns: Name, Type, NotNull, DefaultValue, IsPrimaryKey

// Enable WAL mode
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
|> Sql.vacuum  // Rebuild database

connectionString()
|> Sql.connect
|> Sql.analyze  // Update statistics"""
        ]

        // Bulk Operations
        Html.h3 [ prop.className "text-2xl font-bold mt-8 mb-4"; prop.text "Bulk Operations" ]
        Html.divClassed "description" [
            Html.text "Efficient bulk data operations:"
        ]
        Html.divClassed "max-w-2xl" [
            linedMockupCode """// Bulk delete by column values
connectionString()
|> Sql.connect
|> Sql.bulkDelete "Users" "Id" [1; 2; 3]

// Upsert (INSERT OR REPLACE)
type User = { Id: int; Name: string }

let users = [
    { Id = 1; Name = "John" }
    { Id = 2; Name = "Jane" }
]

connectionString()
|> Sql.connect
|> Sql.upsert "Users" users"""
        ]
    ]
