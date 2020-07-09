module TestSqlite

open System
open System.Data

open Fumble

// get the connection from the environment
let connectionString() =
    @"Data Source=C:\datalog\database\datarecords.db"

let getRecords =
    let query =
            """
            SELECT TimeStamp, MeterId, RowKey, MeterType, Value, ReadingError
            FROM `DataRecords`;
            """
    printfn "Query %A" query
    connectionString()
    |> Sqlite.connect
    |> Sqlite.query query
    |> Sqlite.execute (fun read ->
        {|
            TimeStamp = read.dateTime "TimeStamp"
            MeterId = read.string "MeterId"
            RowKey = read.string "RowKey"
            MeterType = read.string "MeterType"
            Value = read.floatOrNone "Value"
            ReadingError = read.stringOrNone "ReadingError"
         |})
    |> function
    | Ok r -> r |> Seq.toArray
    | Error error -> failwithf "Expected result %A"  error
