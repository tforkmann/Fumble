namespace Fumble

module Evaluators =
    open Fumble.GenericDeconstructor
    open Fumble.Reflection
    open System.Linq
    let private specialStrings = [ "*" ]

    let private inBrackets (s:string) =
        s.Split('.')
        |> Array.map (fun x -> if specialStrings.Contains(x) then x else sprintf "[%s]" x)
        |> String.concat "."

    let private safeTableName schema table =
        match schema, table with
        | None, table -> table |> inBrackets
        | Some schema, table -> (schema |> inBrackets) + "." + (table |> inBrackets)
    let evalInsertQuery fields _ (q: InsertQuery<_>) =
        let fieldNames =
            fields
            |> List.map inBrackets
            |> String.concat ", "
            |> sprintf "(%s)"

        let values =
            q.Values
            |> List.mapi (fun i _ ->
                fields
                |> List.map (fun field -> sprintf "@%s" field)
                |> String.concat ", "
                |> sprintf "(%s)")
            |> String.concat ", "

        sprintf "INSERT INTO %s %s VALUES %s" (safeTableName q.Schema q.Table) fieldNames values
    let evalCreateQuery fields _ (q: InsertQuery<_>) =
        let fieldNames =
            fields
            |> List.map inBrackets
            |> String.concat ", "
            |> sprintf "(%s)"

        // let values =
        //     q.Values
        //     |> List.mapi (fun i x ->
        //         fields
        //         |> List.map (fun field -> sprintf "@%s" field)
        //         |> String.concat ", "
        //         |> sprintf "(%s)")
        //     |> String.concat ", "

        sprintf "CREATE TABLE IF NOT EXISTS %s %s" (safeTableName q.Schema q.Table) fieldNames

        // "CREATE TABLE IF NOT EXISTS [Plant] (
        //             [TimeStamp] varchar(20),
        //             [MeterId] varchar(20),
        //             [RowKey] varchar(20),
        //             [MeterType] varchar(20),
        //             [Value] float NULL,
        //             [ReadingError] varchar(20) NULL
        //         )"

module SqliteDeconstructor =
    open Fumble.GenericDeconstructor

    [<AbstractClass; Sealed>]
    type Deconstructor =
        static member insert(q: InsertQuery<'a>) = q |> insert Evaluators.evalInsertQuery
        static member create(q: InsertQuery<'a>) = q |> create Evaluators.evalCreateQuery

