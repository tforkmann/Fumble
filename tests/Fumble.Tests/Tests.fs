module Tests

open System
open System.Data
open Expecto
open Fumble
open Microsoft.Data.Sqlite

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
                testDatabase "Add records into trade table"
                <| fun connectionStringMemory ->
                    connectionStringMemory
                    |> Sqlite.connect
                    |> Sqlite.command "insert into Trades(symbol, timestamp, price, tradesize)
                        values (@Symbol, @Timestamp, @Price, @TradeSize)"
                    |> Sqlite.insertData trades
                    |> function
                    | Ok x ->
                        printfn "rows affected %A" (x |> List.sum)
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail () ]
          testList "Create and insert"
              [ testDatabase "Query all records from the trade table"
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
                        fail () ] ]


//   testDatabase "Iterating over the rows works"
//   <| fun connectionString ->
//       let rows = ResizeArray<int * string>()
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select * from (values (1, N'john')) as users(id, username)"
//       |> Sqlite.iter (fun read -> rows.Add(read.int "id", read.string "username"))
//       |> function
//       | Ok () ->
//           Expect.equal 1 (fst rows.[0]) "Number is read correctly"
//           Expect.equal "john" (snd rows.[0]) "String is read correctly"
//       | otherwise -> fail ()

//   testDatabase "Reading a parameterized query"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select * from (values (@userId, @username)) as users(id, username)"
//       |> Sqlite.parameters
//           [ "@userId", Sqlite.int 5
//             "@username", Sqlite.string "jane" ]
//       |> Sqlite.execute (fun read -> read.int "id", read.string "username")
//       |> function
//       | Ok [ (5, "jane") ] -> pass ()
//       | otherwise -> fail ()

//   testDatabase "Reading date time as scalar"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select getdate() as now"
//       |> Sqlite.execute (fun read -> read.dateTime "now")
//       |> function
//       | Ok [ time ] -> pass ()
//       | otherwise -> fail ()

//   testDatabase "Sqlite.execute catches exceptions"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select some invalid SQL"
//       |> Sqlite.execute (fun read -> None)
//       |> function
//       | Error ex -> pass ()
//       | Ok _ -> fail ()

//   testDatabase "Executing stored procedure"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.storedProcedure "sp_executesql"
//       |> Sqlite.parameters [ "@stmt", Sqlite.string "SELECT 42 AS A" ]
//       |> Sqlite.execute (fun read -> read.int "A")
//       |> function
//       | Ok [ 42 ] -> pass ()
//       | Error error -> raise error
//       | otherwise -> fail ()

//   testDatabase "Executing parameterized function in query"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "SELECT LOWER(@inputText) as output"
//       |> Sqlite.parameters [ "@inputText", Sqlite.string "TEXT" ]
//       |> Sqlite.execute (fun read -> read.string "output")
//       |> function
//       | Ok [ "text" ] -> pass ()
//       | otherwise -> fail ()

//   testDatabase "binary roundtrip"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select @blob as blob"
//       |> Sqlite.parameters [ "@blob", Sqlite.bytes [| byte 1; byte 2; byte 3 |] ]
//       |> Sqlite.execute (fun read -> read.bytes "blob")
//       |> function
//       | Ok [ bytes ] -> Expect.equal bytes [| byte 1; byte 2; byte 3 |] "bytes are the same"
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "reading unique identifier works"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select newid() as uid"
//       |> Sqlite.execute (fun read -> read.uniqueidentifier "uid")
//       |> function
//       | Ok [ value ] -> pass ()
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "unique identifier roundtrip"
//   <| fun connectionString ->
//       let original = Guid.NewGuid()
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select @identifier as value"
//       |> Sqlite.parameters [ "@identifier", Sqlite.uniqueidentifier original ]
//       |> Sqlite.execute (fun read -> read.uniqueidentifier "value")
//       |> function
//       | Ok [ roundtripped ] -> Expect.equal original roundtripped "Roundtrip works"
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "unique identifier roundtrip from string"
//   <| fun connectionString ->
//       let original = Guid.NewGuid()
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select @identifier as value"
//       |> Sqlite.parameters [ "@identifier", Sqlite.uniqueidentifier original ]
//       |> Sqlite.execute (fun read -> read.string "value")
//       |> function
//       | Ok [ roundtripped ] -> Expect.equal (string original) roundtripped "Roundtrip works"
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "Simpy reading a single column from table"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select name from (values(N'one'), (N'two'), (N'three')) as numbers(name)"
//       |> Sqlite.execute (fun read -> read.string "name")
//       |> function
//       | Ok [ "one"; "two"; "three" ] -> pass ()
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "reading count as int64"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select count(*) from (values(1, 2)) as numbers(one, two)"
//       |> Sqlite.execute (fun read -> read.int64 0)
//       |> function
//       | Ok [ 1L ] -> pass ()
//       | Ok otherwise -> fail ()
//       | Error ex -> raise ex

//   testDatabase "reading count as int32"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select count(*) from (values(1, 2)) as numbers(one, two)"
//       |> Sqlite.execute (fun read -> read.int 0)
//       |> function
//       | Ok [ 1 ] -> pass ()
//       | Ok otherwise -> fail ()
//       | Error ex -> raise ex

//   testDatabase "decimal roundtrip"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select @value as value"
//       |> Sqlite.parameters [ "@value", Sqlite.decimal 1.234567M ]
//       |> Sqlite.execute (fun read -> read.decimal "value")
//       |> function
//       | Ok [ value ] -> Expect.equal value 1.234567M "decimal is correct"
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "reading double as decimal"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "select @value as value"
//       |> Sqlite.parameters [ "@value", Sqlite.double 1.2345 ]
//       |> Sqlite.execute (fun read -> read.decimal "value")
//       |> function
//       | Ok [ value ] -> Expect.equal value 1.2345M "decimal is correct"
//       | Error ex -> raise ex
//       | otherwise -> fail ()

//   testDatabase "reading tinyint as int"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "SELECT CAST(@value as tinyint) as output"
//       |> Sqlite.parameters [ "value", Sqlite.int 1 ]
//       |> Sqlite.execute (fun read -> read.int "output")
//       |> function
//       | Ok [ 1 ] -> pass ()
//       | Error error -> raise error
//       | otherwise -> fail ()

//   testDatabase "reading tinyint as int64"
//   <| fun connectionString ->
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query "SELECT CAST(@value as tinyint) as output"
//       |> Sqlite.parameters [ "value", Sqlite.int 1 ]
//       |> Sqlite.execute (fun read -> read.int64 "output")
//       |> function
//       | Ok [ 1L ] -> pass ()
//       | Error error -> raise error
//       | otherwise -> fail ()

//   testDatabase "table-valued parameters in a stored procedure"
//   <| fun connectionString ->
//       // create a custom SQL type
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query
//           "create type CustomPeopleTableType as table (firstName nvarchar(max), lastName nvarchar(max))"
//       |> Sqlite.executeNonQuery
//       |> ignore

//       // create a stored procedure to use the custom SQL type
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.query """
//     create proc sp_TableValuedParametersTest
//         (@people CustomPeopleTableType READONLY)
//     as
//     begin
//         select firstName, lastName from @people
//     end
// """
//       |> Sqlite.executeNonQuery
//       |> ignore

//       // create a new table-valued parameter
//       let people =
//           let customTypeName = "CustomPeopleTableType"
//           let table = new DataTable()
//           table.Columns.Add "firstName" |> ignore
//           table.Columns.Add "lastName" |> ignore
//           table.Rows.Add("John", "Doe") |> ignore
//           table.Rows.Add("Jane", "Doe") |> ignore
//           table.Rows.Add("Fred", "Doe") |> ignore
//           Sqlite.table (customTypeName, table)

//       // query the procedure
//       connectionString
//       |> Sqlite.connect
//       |> Sqlite.storedProcedure "sp_TableValuedParametersTest"
//       |> Sqlite.parameters [ "@people", people ]
//       |> Sqlite.execute (fun read ->
//           read.string "firstName"
//           + " "
//           + read.string "lastName")
//       |> function
//       | Ok [ first; second; third ] ->
//           Expect.equal first "John Doe" "First name is John Doe"
//           Expect.equal second "Jane Doe" "Second name is Jane Doe"
//           Expect.equal third "Fred Doe" "Third name is Fred Doe"
//       | Error ex -> raise ex
//       | otherwise -> fail () ]
