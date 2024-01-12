module Tests

open System
open Expecto
open Fumble

let pass () = Expect.isTrue true "true is true :)"
let fail () = Expect.isTrue false "true is false :("

let testDatabase testName f =
    testCase testName <| fun _ -> f @"Data Source=trades.db"

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
      SingleValue: float32 option
      Error: string option }

type Width = { Width: float }

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
    { ClientStatus = "Running"
      TimeStamp = DateTimeOffset.Now
      ErrorCode = None }

let width = { Width = 150 }

let sqlRecords =
    [ { TimeStamp = "2024-01-01" |> string
        MeterId = "74-1-1"
        RowKey = "9854577851"
        MeterType = "Zählerwert"
        Value = None
        SingleValue = None
        Error = Some "Error" }
      { TimeStamp = "2024-01-01" |> string
        MeterId = "74-1-2"
        RowKey = "9854577851"
        MeterType = "Messwert"
        Value = Some 45.
        SingleValue = Some 45.f
        Error = None } ]

let props = tradesSingle.GetType().GetProperties()

[<Tests>]
let tests =
    testList
        "Fumble"
        [ testList
              "Create and insert"
              [ testDatabase "Create trade table"
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
                            fail () ]


          testList
              "Read records"
              [ testDatabase "Query all records from the trade table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sql.connect
                    |> Sql.query
                        """
                    SELECT * FROM Trades
                    ORDER BY timestamp desc
                    """
                    |> Sql.execute (fun read ->
                        { Symbol = read.string "Symbol"
                          Timestamp = read.dateTime "Timestamp"
                          Price = read.double "Price"
                          TradeSize = read.double "TradeSize" })
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
                    |> Sql.execute (fun read ->
                        { TimeStamp = read.string "TimeStamp"
                          MeterId = read.string "MeterId"
                          RowKey = read.string "RowKey"
                          MeterType = read.string "MeterType"
                          Value = read.floatOrNone "Value"
                          SingleValue = read.float32OrNone "SingleValue"
                          Error = read.stringOrNone "Error" })
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
                            fail () ]
          testList
              "Compare results records"
              [ testCase "Inserted records should be the same as queried records"
                <| fun _ ->
                    let expected = sqlRecords

                    let actual =
                        connectionStringMemory
                        |> Sql.connect
                        |> Sql.query
                            """
                    SELECT * FROM SqlRecords
                    """
                        |> Sql.execute (fun read ->
                            { TimeStamp = read.string "TimeStamp"
                              MeterId = read.string "MeterId"
                              RowKey = read.string "RowKey"
                              MeterType = read.string "MeterType"
                              Value = read.floatOrNone "Value"
                              SingleValue = read.float32OrNone "Value"
                              Error = read.stringOrNone "Error" })
                        |> function
                            | Ok x ->
                                printfn "queried data %A" x
                                x |> List.distinct
                            | otherwise ->
                                printfn "error %A" otherwise
                                []

                    Expect.equal actual expected "Result should be true" ] ]

let config =
    { defaultConfig with
        runInParallel = false }

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
