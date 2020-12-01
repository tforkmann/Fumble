module Tests

open System
open Expecto
open Fumble

let pass () = Expect.isTrue true "true is true :)"
let fail () = Expect.isTrue false "true is false :("

let testDatabase testName f =
    testCase testName
    <| fun _ -> f @"Data Source=trades.db"

let connectionStringMemory = sprintf @"Data Source=trades.db"

type TradeData =
    { Symbol: string
      Timestamp: DateTime
      Price: float
      TradeSize: float }
type Status =
    { ClientStatus: string
      TimeStamp: DateTimeOffset
      ErrorCode: int option }
type SqlRecord =
    { TimeStamp: string
      MeterId: string
      RowKey: string
      MeterType: string
      Value: float option
      Error: string option }


// Sample Data
let trades =
    [ { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 33)
        Price = 2751.20
        TradeSize = 0.01000000 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21)
        Price = 2750.20
        TradeSize = 0.01000000 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21)
        Price = 2750.01
        TradeSize = 0.40000000 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 21)
        Price = 2750.01
        TradeSize = 0.55898959 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 03)
        Price = 2750.00
        TradeSize = 0.86260000 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 44, 03)
        Price = 2750.00
        TradeSize = 0.03000000 }
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2017, 07, 28, 10, 43, 31)
        Price = 2750.01
        TradeSize = 0.44120000 } ]
let tradesSingle =
      { Symbol = "BTC/USD"
        Timestamp = new DateTime(2020, 07, 28, 10, 43, 31)
        Price = 2750.01
        TradeSize = 0.44120000 }
let status =
      {   ClientStatus = "Running"
          TimeStamp = DateTimeOffset.Now
          ErrorCode = None }

let sqlRecords =
    [
    { TimeStamp = DateTimeOffset.Now |> string
      MeterId = "74-1-1"
      RowKey = "9854577851"
      MeterType = "Zählerwert"
      Value = None
      Error = Some "Error"  }
    { TimeStamp = DateTimeOffset.Now |> string
      MeterId = "74-1-2"
      RowKey = "9854577851"
      MeterType = "Messwert"
      Value = Some 45.
      Error = None  }
      ]

let props = tradesSingle.GetType().GetProperties()

[<Tests>]
let tests =
    testList "Fumble"
        [ testList "Create and insert"
              [ testDatabase "Create trade table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.command "Create Table Trades (
                        Symbol varchar(20),
                        Timestamp datetime,
                        Price float,
                        TradeSize float)"
                    |> Sqlite.executeCommand
                    |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
                testDatabase "Create status table with optional status"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.command "CREATE TABLE IF NOT EXISTS [Status] (
                            [ClientStatus] varchar(20),
                            [ErrorCode] int NULL,
                            [TimeStamp] datetimeoffset
                        )"
                    |> Sqlite.executeCommand
                    |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
                testDatabase "Create sqlrecords table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.command
                        "CREATE TABLE IF NOT EXISTS [SqlRecords] (
                            [TimeStamp] varchar(20),
                            [MeterId] varchar(20),
                            [RowKey] varchar(20),
                            [MeterType] varchar(20),
                            [Value] float NULL,
                            [Error] varchar(20) NULL
                        )"
                    |> Sqlite.executeCommand
                    |> function
                    | Ok x -> pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
                testDatabase "Add option record into status table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.commandInsert<Status> ("Status",[status])
                    |> Sqlite.insertData [status]
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
                    |> Sqlite.connect
                    |> Sqlite.commandInsert<TradeData> ("Trades",trades)
                    |> Sqlite.insertData trades
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
                    |> Sqlite.connect
                    |> Sqlite.commandInsert<SqlRecord> ("SqlRecords",sqlRecords)
                    |> Sqlite.insertData sqlRecords
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
                    |> Sqlite.connect
                    |> Sqlite.commandInsert<TradeData> ("Trades",[tradesSingle])
                    |> Sqlite.insertData [tradesSingle]
                    |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail ()
                ]
          testList "Read records"
              [
                testDatabase "Query all records from the trade table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.query """
                    SELECT * FROM Trades
                    ORDER BY timestamp desc
                    """
                    |> Sqlite.execute (fun read ->
                        { Symbol = read.string "Symbol"
                          Timestamp = read.dateTime "Timestamp"
                          Price = read.double "Price"
                          TradeSize = read.double "TradeSize" })
                    |> function
                    | Ok x ->
                        printfn "queried data %A" x
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                //         fail ()
                testDatabase "Query all sqlrecords from the sql table"
                    <| fun connectionStringMemory ->
                        connectionStringMemory
                        |> Sqlite.connect
                        |> Sqlite.query """
                        SELECT * FROM SqlRecords
                        """
                        |> Sqlite.execute (fun read ->
                             { TimeStamp = read.string "TimeStamp"
                               MeterId = read.string "MeterId"
                               RowKey = read.string "RowKey"
                               MeterType = read.string "MeterType"
                               Value = read.doubleOrNone "Value"
                               Error = read.stringOrNone "Error" })
                        |> function
                        | Ok x ->
                            // printfn "queried data %A" x
                            pass ()
                        | otherwise ->
                            printfn "error %A" otherwise
                            fail ()
                         ]
          testList "Compare results records" [
            testCase "Inserted records should be the same as queried records" <| fun _ ->
                let expected = sqlRecords
                let actual =
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.query """
                    SELECT * FROM SqlRecords
                    """
                    |> Sqlite.execute (fun read ->
                         { TimeStamp = read.string "TimeStamp"
                           MeterId = read.string "MeterId"
                           RowKey = read.string "RowKey"
                           MeterType = read.string "MeterType"
                           Value = read.doubleOrNone "Value"
                           Error = read.stringOrNone "Error" })
                    |> function
                    | Ok x ->
                        printfn "queried data %A" x
                        x |> List.distinct
                    | otherwise ->
                        printfn "error %A" otherwise
                        []
                Expect.equal actual expected "Result should be true"
            ]             ]

let config =
        { defaultConfig with
            runInParallel = false }
[<EntryPoint>]
let main argv =
    runTestsInAssembly config argv
