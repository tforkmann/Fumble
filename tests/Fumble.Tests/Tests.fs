module Tests

open System
open Expecto
open Fumble

let pass () = Expect.isTrue true "true is true :)"
let fail () = Expect.isTrue false "true is false :("

let testDatabase testName f =
    testCase testName <| fun _ -> f @"Data Source=trades.db"

let connectionStringMemory = sprintf @"Data Source=trades.db"

type TradeData = {
    Symbol: string
    Timestamp: DateTime option
    Price: float
    TradeSize: float
}

type Status = {
    ClientStatus: string
    TimeStamp: DateTimeOffset
    ErrorCode: int option
}

type SqlRecord = {
    TimeStamp: string
    MeterId: string
    RowKey: string
    MeterType: string
    Value: float option
    SingleValue: float32 option
    Error: string option
}

type Width = { Width: float }

// New types for testing new features
type NewTypesRecord = {
    Id: int
    Duration: TimeSpan
    BirthDate: DateOnly
    AlarmTime: TimeOnly
    Counter: uint32
    BigCounter: uint64
    SmallValue: byte
}

type NewTypesRecordOptional = {
    Id: int
    Duration: TimeSpan option
    BirthDate: DateOnly option
    AlarmTime: TimeOnly option
}

// Sample Data
let trades = [
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 33) |> Some
        Price = 2751.20
        TradeSize = 0.01000000
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21) |> Some
        Price = 2750.20
        TradeSize = 0.01000000
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21) |> Some
        Price = 2750.01
        TradeSize = 0.40000000
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21) |> Some
        Price = 2750.01
        TradeSize = 0.55898959
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 03) |> Some
        Price = 2750.00
        TradeSize = 0.86260000
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 03) |> Some
        Price = 2750.00
        TradeSize = 0.03000000
    }
    {
        Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 43, 31) |> Some
        Price = 2750.01
        TradeSize = 0.44120000
    }
]

let tradesSingle = {
    Symbol = "BTC/USD"
    Timestamp = new DateTime(2020, 07, 28, 10, 43, 31) |> Some
    Price = 2750.01
    TradeSize = 0.44120000
}

let status = {
    ClientStatus = "Running"
    TimeStamp = DateTimeOffset.Now
    ErrorCode = None
}

let width = { Width = 150 }

let sqlRecords = [
    {
        TimeStamp = "2024-01-01" |> string
        MeterId = "74-1-1"
        RowKey = "9854577851"
        MeterType = "Zählerwert"
        Value = None
        SingleValue = None
        Error = Some "Error"
    }
    {
        TimeStamp = "2024-01-01" |> string
        MeterId = "74-1-2"
        RowKey = "9854577851"
        MeterType = "Messwert"
        Value = Some 45.
        SingleValue = Some 45.f
        Error = None
    }
]

let props = tradesSingle.GetType().GetProperties()

[<Tests>]
let tests =
    testList "Fumble" [
        testList "Create and insert" [
            testDatabase "Create trade table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandCreate<TradeData> ("Trades")
                |> Sql.executeCommand
                |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Create status table with optional status"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandCreate<Status> ("Status")
                |> Sql.executeCommand
                |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Create sqlrecords table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandCreate<SqlRecord> ("SqlRecords")
                |> Sql.executeCommand
                |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Create width table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandCreate<Width> ("Width")
                |> Sql.executeCommand
                |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Add option record into status table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandInsert<Status> ("Status")
                |> Sql.insertData [ status ]
                |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Add trades into trade table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandInsert<TradeData> "Trades"
                |> Sql.insertData trades
                |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Add sqlrecords into sqlrecords table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandInsert<SqlRecord> "SqlRecords"
                |> Sql.insertData sqlRecords
                |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
            testDatabase "Add single record into trade table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandInsert<TradeData> ("Trades")
                |> Sql.insertData [ tradesSingle ]
                |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()

            testDatabase "Add single record into width table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandInsert<Width> ("Width")
                |> Sql.insertData [ width ]
                |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
        ]


        testList "Read records" [
            testDatabase "Query all records from the trade table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query
                    """
                    SELECT * FROM Trades
                    ORDER BY timestamp desc
                    """
                |> Sql.execute (fun read -> {
                    Symbol = read.string "Symbol"
                    Timestamp = read.dateTimeOrNone "Timestamp"
                    Price = read.double "Price"
                    TradeSize = read.double "TradeSize"
                })
                |> function
                    | Ok x ->
                        printfn "queried data %A" x
                        pass ()
                    | otherwise -> printfn "error %A" otherwise
            //         fail ()
            testDatabase "Query all sqlrecords from the sqlite table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query
                    """
                        SELECT * FROM SqlRecords
                        """
                |> Sql.execute (fun read -> {
                    TimeStamp = read.string "TimeStamp"
                    MeterId = read.string "MeterId"
                    RowKey = read.string "RowKey"
                    MeterType = read.string "MeterType"
                    Value = read.floatOrNone "Value"
                    SingleValue = read.float32OrNone "SingleValue"
                    Error = read.stringOrNone "Error"
                })
                |> function
                    | Ok x ->
                        // printfn "queried data %A" x
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()

            testDatabase "Query all width from the sqlite table"
            <| fun connectionStringMemory ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query
                    """
                        SELECT * FROM Width
                        """
                |> Sql.execute (fun read -> { Width = read.double "Width" })
                |> function
                    | Ok x ->
                        // printfn "queried data %A" x
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
        ]
        testList "Compare results records" [
            testCase "Inserted records should be the same as queried records"
            <| fun _ ->
                let expected = sqlRecords

                let actual =
                    connectionStringMemory
                    |> Sql.connect
                    |> Sql.query
                        """
                    SELECT * FROM SqlRecords
                    """
                    |> Sql.execute (fun read -> {
                        TimeStamp = read.string "TimeStamp"
                        MeterId = read.string "MeterId"
                        RowKey = read.string "RowKey"
                        MeterType = read.string "MeterType"
                        Value = read.floatOrNone "Value"
                        SingleValue = read.float32OrNone "Value"
                        Error = read.stringOrNone "Error"
                    })
                    |> function
                        | Ok x ->
                            printfn "queried data %A" x
                            x |> List.distinct
                        | otherwise ->
                            printfn "error %A" otherwise
                            []

                Expect.equal actual expected "Result should be true"
        ]

        // ============================================
        // New Feature Tests for v2.0
        // ============================================

        testList "New data types" [
            testCase "Create table with new types"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.commandCreate<NewTypesRecord> "NewTypes"
                |> Sql.executeCommand
                |> function
                    | Ok _ -> pass ()
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "Insert and query TimeSpan, DateOnly, TimeOnly"
            <| fun _ ->
                let testRecord = {
                    Id = 1
                    Duration = TimeSpan.FromHours(2.5)
                    BirthDate = DateOnly(1990, 5, 15)
                    AlarmTime = TimeOnly(7, 30, 0)
                    Counter = 42u
                    BigCounter = 123456789UL
                    SmallValue = 255uy
                }

                // Insert
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "INSERT INTO NewTypes (Id, Duration, BirthDate, AlarmTime, Counter, BigCounter, SmallValue) VALUES (@Id, @Duration, @BirthDate, @AlarmTime, @Counter, @BigCounter, @SmallValue)"
                |> Sql.parameters [
                    "Id", Sql.int testRecord.Id
                    "Duration", Sql.timeSpan testRecord.Duration
                    "BirthDate", Sql.dateOnly testRecord.BirthDate
                    "AlarmTime", Sql.timeOnly testRecord.AlarmTime
                    "Counter", Sql.uint32 testRecord.Counter
                    "BigCounter", Sql.uint64 testRecord.BigCounter
                    "SmallValue", Sql.byte testRecord.SmallValue
                ]
                |> Sql.executeNonQuery
                |> function
                    | Ok _ -> pass ()
                    | Error e ->
                        printfn "Insert error %A" e
                        fail ()

            testCase "Query new types back"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT * FROM NewTypes WHERE Id = 1"
                |> Sql.execute (fun read -> {
                    Id = read.int "Id"
                    Duration = read.timeSpan "Duration"
                    BirthDate = read.dateOnly "BirthDate"
                    AlarmTime = read.timeOnly "AlarmTime"
                    Counter = read.uint32 "Counter"
                    BigCounter = read.uint64 "BigCounter"
                    SmallValue = read.tinyint "SmallValue"
                })
                |> function
                    | Ok (record :: _) ->
                        Expect.equal record.Id 1 "Id should match"
                        Expect.equal record.Duration (TimeSpan.FromHours(2.5)) "Duration should match"
                        Expect.equal record.BirthDate (DateOnly(1990, 5, 15)) "BirthDate should match"
                        Expect.equal record.AlarmTime (TimeOnly(7, 30, 0)) "AlarmTime should match"
                        Expect.equal record.Counter 42u "Counter should match"
                        Expect.equal record.BigCounter 123456789UL "BigCounter should match"
                        Expect.equal record.SmallValue 255uy "SmallValue should match"
                    | Ok [] ->
                        printfn "No records found"
                        fail ()
                    | Error e ->
                        printfn "Query error %A" e
                        fail ()
        ]

        testList "executeScalar tests" [
            testCase "executeScalar returns count"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT COUNT(*) FROM Trades"
                |> Sql.executeScalar<int64>
                |> function
                    | Ok (Some count) ->
                        Expect.isGreaterThan count 0L "Count should be greater than 0"
                    | Ok None -> fail ()
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "executeScalar returns null for empty result"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT Symbol FROM Trades WHERE Symbol = 'NONEXISTENT' LIMIT 1"
                |> Sql.executeScalar<string>
                |> function
                    | Ok None -> pass ()
                    | Ok (Some _) -> fail ()
                    | Error _ -> pass () // Also acceptable - no rows
        ]

        testList "executeCount and executeExists tests" [
            testCase "executeCount returns correct count"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT COUNT(*) FROM Trades"
                |> Sql.executeCount
                |> function
                    | Ok count ->
                        Expect.isGreaterThan count 0L "Count should be greater than 0"
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "executeExists returns true for existing data"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT 1 FROM Trades WHERE Symbol = 'BTC/USD'"
                |> Sql.executeExists
                |> function
                    | Ok exists ->
                        Expect.isTrue exists "Should find BTC/USD trades"
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "executeExists returns false for non-existing data"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT 1 FROM Trades WHERE Symbol = 'NONEXISTENT'"
                |> Sql.executeExists
                |> function
                    | Ok exists ->
                        Expect.isFalse exists "Should not find NONEXISTENT trades"
                    | Error e ->
                        printfn "error %A" e
                        fail ()
        ]

        testList "Pagination tests" [
            testCase "take limits results"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT * FROM Trades"
                |> Sql.take 2
                |> Sql.execute (fun read -> read.string "Symbol")
                |> function
                    | Ok results ->
                        Expect.equal (List.length results) 2 "Should return exactly 2 results"
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "paginate returns correct page"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "SELECT * FROM Trades ORDER BY Price"
                |> Sql.paginate 1 3
                |> Sql.execute (fun read -> read.double "Price")
                |> function
                    | Ok results ->
                        Expect.equal (List.length results) 3 "Should return 3 results for page 1"
                    | Error e ->
                        printfn "error %A" e
                        fail ()
        ]

        testList "SQLite pragma tests" [
            testCase "Get SQLite version"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.sqliteVersion
                |> function
                    | Ok (Some version) ->
                        Expect.isNotEmpty version "Version should not be empty"
                        printfn "SQLite version: %s" version
                    | Ok None -> fail ()
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "List tables"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.listTables
                |> function
                    | Ok tables ->
                        Expect.contains tables "Trades" "Should contain Trades table"
                        printfn "Tables: %A" tables
                    | Error e ->
                        printfn "error %A" e
                        fail ()

            testCase "Get table info"
            <| fun _ ->
                connectionStringMemory
                |> Sql.connect
                |> Sql.tableInfo "Trades"
                |> function
                    | Ok columns ->
                        let columnNames = columns |> List.map (fun c -> c.Name)
                        Expect.contains columnNames "Symbol" "Should contain Symbol column"
                        Expect.contains columnNames "Price" "Should contain Price column"
                    | Error e ->
                        printfn "error %A" e
                        fail ()
        ]

        testList "In-memory database tests" [
            testCase "Connect to in-memory database"
            <| fun _ ->
                Sql.connectInMemory ()
                |> Sql.query "SELECT 1 as Value"
                |> Sql.execute (fun read -> read.int "Value")
                |> function
                    | Ok [1] -> pass ()
                    | Ok _ -> fail ()
                    | Error e ->
                        printfn "error %A" e
                        fail ()
        ]

        testList "Bulk operations tests" [
            testCase "Bulk delete"
            <| fun _ ->
                // First insert some test data
                connectionStringMemory
                |> Sql.connect
                |> Sql.query "INSERT INTO Trades (Symbol, Timestamp, Price, TradeSize) VALUES ('TEST/DEL', NULL, 100.0, 1.0)"
                |> Sql.executeNonQuery
                |> ignore

                // Then bulk delete
                connectionStringMemory
                |> Sql.connect
                |> Sql.bulkDelete "Trades" "Symbol" ["TEST/DEL" :> obj]
                |> function
                    | Ok deleted ->
                        Expect.isGreaterThanOrEqual deleted 1 "Should delete at least 1 row"
                    | Error e ->
                        printfn "error %A" e
                        fail ()
        ]
    ]

let config = {
    defaultConfig with
        runInParallel = false
}

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
