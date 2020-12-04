namespace Fumble

module InsertBuilder =
    open GenericDeconstructor
    open Fumble.SqliteDeconstructor
    type InsertBuilder<'a>() =
        member __.Yield _ =
            {
                Table = ""
            } : InsertQuery<'a>

        /// Sets the TABLE name for query
        [<CustomOperation "table">]
        member __.Table (state:InsertQuery<_>, name) = { state with Table = name }

    let insert<'a> = InsertBuilder<'a>()
    let createInsertString<'a> (tableName) =
        insert<'a> {
                table tableName
        }
        |> Deconstructor.insert |> fst
    let createCreateString<'a> tableName =
        insert<'a> {
                table tableName
        }
        |> Deconstructor.create
