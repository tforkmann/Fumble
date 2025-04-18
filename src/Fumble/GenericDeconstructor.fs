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
                    | "System.String" -> "varchar(20)"
                    | "System.Int32" -> "int"
                    | "System.Boolean" -> "int"
                    | "System.Single" -> "float32"
                    | "System.Double" -> "float"
                    | "System.DateTime" -> "datetime"
                    | "System.DateTimeOffset" -> "datetimeoffset"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.String]" -> "varchar(20) NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Single]" -> "float32 NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Double]" -> "float NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.Int32]" -> "int NULL"
                    | "Microsoft.FSharp.Core.FSharpOption`1[System.DateTime]" -> "varchar(20) NULL"
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
