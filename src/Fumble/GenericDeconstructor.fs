namespace Fumble

module GenericDeconstructor =
    open System

    type InsertQuery<'a> = { Table: string }

    let private _insert evalInsertQuery (q: InsertQuery<'a>) fields outputFields =
        let query: string = evalInsertQuery fields outputFields q

        let pars =
            typeof<'a>
            |> Reflection.getFields
            |> List.map (fun key -> sprintf "%s" key)
            |> String.concat ", "

        query, pars

    let insert evalInsertQuery (q: InsertQuery<'a>) =
        let fields = typeof<'a> |> Reflection.getFields
        _insert evalInsertQuery q fields []

    let private _create evalCreateQuery (q: InsertQuery<'a>) fields (properties: Type list) outputFields =
        let query: string = evalCreateQuery fields outputFields q

        let pars =
            typeof<'a>
            |> Reflection.getFields
            |> List.mapi (fun i key ->

                let property =
                    match properties.[i].ToString() with
                    | "System.String" -> "TEXT"
                    | "System.Int32" -> "INTEGER"
                    | "System.Int16" -> "INTEGER"
                    | "System.Int64" -> "INTEGER"
                    | "System.UInt32" -> "INTEGER"
                    | "System.UInt64" -> "INTEGER"
                    | "System.Byte" -> "INTEGER"
                    | "System.Boolean" -> "INTEGER"
                    | "System.Single" -> "REAL"
                    | "System.Double" -> "REAL"
                    | "System.Decimal" -> "REAL"
                    | "System.DateTime" -> "TEXT"
                    | "System.DateTimeOffset" -> "TEXT"
                    | "System.TimeSpan" -> "INTEGER"
                    | "System.DateOnly" -> "TEXT"
                    | "System.TimeOnly" -> "INTEGER"
                    | "System.Guid" -> "TEXT"
                    | "System.Byte[]" -> "BLOB"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.String]" -> "TEXT NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Single]" -> "REAL NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Double]" -> "REAL NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Decimal]" -> "REAL NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Int32]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Int16]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Int64]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.UInt32]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.UInt64]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Byte]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Boolean]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.DateTime]" -> "TEXT NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.DateTimeOffset]" -> "TEXT NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.TimeSpan]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.DateOnly]" -> "TEXT NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.TimeOnly]" -> "INTEGER NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Guid]" -> "TEXT NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Byte[]]" -> "BLOB NULL"
                    | x ->
                        failwithf "unmatched property %s" x

                if i = 0 then sprintf "[%s] %s" key property else sprintf "   [%s] %s" key property)
            |> String.concat ",\n "

        [ query; "\n   (" + pars + ")" ]
        |> String.concat ""

    let create evalCreateQuery (q: InsertQuery<'a>) =
        let properties =
            typeof<'a> |> Reflection.getPropertyTypes

        let fields = typeof<'a> |> Reflection.getFields
        _create evalCreateQuery q fields properties []
