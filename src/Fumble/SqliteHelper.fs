namespace Fumble

module Evaluators =
    open Fumble.GenericDeconstructor
    open System.Linq
    let private specialStrings = [ "*" ]

    let private inBrackets (s:string) =
        s.Split('.')
        |> Array.map (fun x -> if specialStrings.Contains(x) then x else sprintf "[%s]" x)
        |> String.concat "."

    let private safeTableName table = table |> inBrackets
    let evalInsertQuery fields _ (q: InsertQuery<'a>) =
        let fieldNames =
            fields
            |> List.map inBrackets
            |> String.concat ", "
            |> sprintf "(%s)"

        let values =
            typeof<'a> |> Reflection.getFields
            |> List.mapi (fun i _ ->
                fields
                |> List.map (fun field -> sprintf "@%s" field)
                |> String.concat ", "
                |> sprintf "(%s)")
            |> List.distinct
            |> String.concat ", "

        sprintf "INSERT INTO %s %s VALUES %s" (safeTableName q.Table) fieldNames values
    let evalCreateQuery fields _ (q: InsertQuery<_>) =
        sprintf "CREATE TABLE IF NOT EXISTS %s" (safeTableName q.Table)

module SqliteDeconstructor =
    open Fumble.GenericDeconstructor

    [<AbstractClass; Sealed>]
    type Deconstructor =
        static member insert(q: InsertQuery<'a>) = q |> insert Evaluators.evalInsertQuery
        static member create(q: InsertQuery<'a>) = q |> create Evaluators.evalCreateQuery

