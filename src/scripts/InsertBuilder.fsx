#load "Sqlite.fsx"
open GenericDeconstructor
type InsertBuilder<'a>() =
    member __.Yield _ =
        {
            Schema = None
            Table = ""
            Values = []
        } : InsertQuery<'a>

    /// Sets the SCHEMA
    [<CustomOperation "schema">]
    member __.Schema (state:InsertQuery<_>, name) = { state with Schema = Some name }

    /// Sets the TABLE name for query
    [<CustomOperation "table">]
    member __.Table (state:InsertQuery<_>, name) = { state with Table = name }

    /// Sets the list of values for INSERT
    [<CustomOperation "values">]
    member __.Values (state:InsertQuery<'a>, values:'a list) = { state with Values = values }

    /// Sets the single value for INSERT
    [<CustomOperation "value">]
    member __.Value (state:InsertQuery<'a>, value:'a) = { state with Values = [value] }

let insert<'a> = InsertBuilder<'a>()
let createInsertString<'a> (tableName,insertValues) =
    insert {
            table tableName
            values insertValues
    }
    |> Deconstructor.insert |> fst


type LogFn = string * Map<string, obj> -> unit
