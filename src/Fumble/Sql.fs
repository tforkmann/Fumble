namespace Fumble

open System
open System.Data
open Microsoft.Data.Sqlite
open System.Threading

type Sql() =
    static member dbnull = SqliteParameter(Value = DBNull.Value)

    static member int(value: int) = SqliteParameter(Value = value, DbType = DbType.Int32)

    static member intOrNone(value: int option) =
        match value with
        | Some value -> Sql.int (value)
        | None -> Sql.dbnull

    static member string(value: string) = SqliteParameter(Value = value, DbType = DbType.String)

    static member stringOrNone(value: string option) =
        match value with
        | Some value -> Sql.string (value)
        | None -> Sql.dbnull

    static member bool(value: bool) = SqliteParameter(Value = value, DbType = DbType.Boolean)

    static member boolOrNone(value: bool option) =
        match value with
        | Some thing -> Sql.bool (thing)
        | None -> Sql.dbnull

    static member double(value: double) = SqliteParameter(Value = value, DbType = DbType.Double)

    static member doubleOrNone(value: double option) =
        match value with
        | Some value -> Sql.double (value)
        | None -> SqliteParameter(Value = DBNull.Value)

    static member decimal(value: decimal) = SqliteParameter(Value = value, DbType = DbType.Decimal)

    static member decimalOrNone(value: decimal option) =
        match value with
        | Some value -> Sql.decimal (value)
        | None -> Sql.dbnull

    static member int16(value: int16) = SqliteParameter(Value = value, DbType = DbType.Int16)

    static member int16OrNone(value: int16 option) =
        match value with
        | Some value -> Sql.int16 value
        | None -> Sql.dbnull

    static member int64(value: int64) = SqliteParameter(Value = value, DbType = DbType.Int64)

    static member int64OrNone(value: int64 option) =
        match value with
        | Some value -> Sql.int64 value
        | None -> Sql.dbnull

    static member dateTime(value: DateTime) = SqliteParameter(Value = value, DbType = DbType.DateTime)

    static member dateTimeOrNone(value: DateTime option) =
        match value with
        | Some value -> Sql.dateTime (value)
        | None -> Sql.dbnull
    static member dateTimeOffset(value: DateTimeOffset) = SqliteParameter(Value=value, DbType = DbType.DateTimeOffset)

    static member dateTimeOffsetOrNone(value: DateTimeOffset option) =
        match value with
        | Some value -> Sql.dateTimeOffset(value)
        | None -> Sql.dbnull
    static member uniqueidentifier(value: Guid) = SqliteParameter(Value = value, DbType = DbType.Guid)

    static member uniqueidentifierOrNone(value: Guid option) =
        match value with
        | Some value -> Sql.uniqueidentifier value
        | None -> Sql.dbnull

    static member bytes(value: byte []) = SqliteParameter(Value = value)

    static member bytesOrNone(value: byte [] option) =
        match value with
        | Some value -> Sql.bytes value
        | None -> Sql.dbnull

    static member table(value: DataTable) = SqliteParameter(Value = value)

    static member parameter(genericParameter: SqliteParameter) = genericParameter

[<RequireQualifiedAccess>]
module Sql =
    let inline tryUnbox<'a> (x:obj) =
        match x with
        | :? 'a as result -> Some (result)
        | :? option<'a> as result when result.IsSome -> result
        | :? option<'a> as result when result.IsNone -> None
        | _ ->
            None
    type SqlProps =
        private { ConnectionString: string
                  SqlQuery: string option
                  SqlCommand: string option
                  Parameters: (string * SqliteParameter) list
                  IsFunction: bool
                  Timeout: int option
                  NeedPrepare: bool
                  CancellationToken: CancellationToken
                  ExistingConnection: SqliteConnection option }

    let private defaultProps () =
        { ConnectionString = ""
          SqlQuery = None
          SqlCommand = None
          Parameters = []
          IsFunction = false
          NeedPrepare = false
          Timeout = None
          CancellationToken = CancellationToken.None
          ExistingConnection = None }

    let connect constr =
        { defaultProps () with
              ConnectionString = constr }

    let existingConnection (connection: SqliteConnection) =
        { defaultProps () with
              ExistingConnection = connection |> Option.ofObj }

    let query (sqlQuery: string) props = { props with SqlQuery = Some sqlQuery }
    let command (sqlCommand: string) props = { props with SqlCommand = Some sqlCommand }
    let commandInsert<'a> (tableName) props =
        let sqlCommand = InsertBuilder.createInsertString<'a> (tableName)
        { props with SqlCommand = Some sqlCommand }
    let commandCreate<'a> (tableName) props =
        let sqlCommand = InsertBuilder.createCreateString<'a> (tableName)
        { props with SqlCommand = Some sqlCommand }

    let queryStatements (sqlQuery: string list) props =
        { props with
              SqlQuery = Some(String.concat "\n" sqlQuery) }

    let storedProcedure (sqlQuery: string) props =
        { props with
              SqlQuery = Some sqlQuery
              IsFunction = true }

    let prepare props = { props with NeedPrepare = true }
    let parameters ls props = { props with Parameters = ls }
    let timeout n props = { props with Timeout = Some n }

    let populateRow (cmd: SqliteCommand) (row: (string * SqliteParameter) list) =
        for (parameterName, parameter) in row do
            // prepend param name with @ if it doesn't already
            let normalizedName =
                if parameterName.StartsWith("@") then parameterName else sprintf "@%s" parameterName

            parameter.ParameterName <- normalizedName
            ignore (cmd.Parameters.Add(parameter))
    let unboxAddValueHelper<'a> value (cmd:SqliteCommand) normalizedName =
        match value |> tryUnbox<'a> with
        | Some x -> cmd.Parameters.AddWithValue(normalizedName,x) |> ignore
        | None -> cmd.Parameters.AddWithValue(normalizedName,DBNull.Value) |> ignore

    let private getConnection (props: SqlProps): SqliteConnection =
        match props.ExistingConnection with
        | Some connection -> connection
        | None -> new SqliteConnection(props.ConnectionString)
    let insertData (insertDataRows: 'a list) (props: SqlProps)=
        try
            let connection = getConnection props
            if not (connection.State.HasFlag ConnectionState.Open)
                then connection.Open()
            use transaction = connection.BeginTransaction()
            let affectedRowsByInsert = ResizeArray<int>()
            for insertData in insertDataRows do
                let cmd =
                    match props.SqlCommand with
                    | Some cmd ->
                    new SqliteCommand(cmd, connection, transaction)
                    | None -> failwith "please add a SqlCommand"
                insertData.GetType().GetProperties()
                |> Array.iter (fun y ->
                    let normalizedName =
                        let parameterName = y.Name
                        if parameterName.StartsWith("@") then parameterName else sprintf "@%s" parameterName

                    let value = y.GetValue(insertData, null)
                    let tOption = typeof<option<obj>>.GetGenericTypeDefinition()
                    match value with
                    | null ->
                        cmd.Parameters.AddWithValue(normalizedName,DBNull.Value) |> ignore
                    | _ when value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() = tOption ->
                            match value.GetType().GenericTypeArguments with
                            | [|t|] when t = typeof<int> ->
                                unboxAddValueHelper<int> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<float> ->
                                unboxAddValueHelper<float> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<float32> ->
                                unboxAddValueHelper<float32> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<string> ->
                                unboxAddValueHelper<string> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<DateTime> ->
                                unboxAddValueHelper<DateTime> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<DateTimeOffset> ->
                                unboxAddValueHelper<DateTimeOffset> value cmd normalizedName|> ignore
                            | [|t|] when t = typeof<obj> -> cmd.Parameters.AddWithValue(normalizedName,t) |> ignore
                            | _   ->  cmd.Parameters.AddWithValue(normalizedName,value) |> ignore

                    | _ ->
                        cmd.Parameters.AddWithValue(normalizedName,value) |> ignore
                )
                let affectedRows = cmd.ExecuteNonQuery()
                affectedRowsByInsert.Add affectedRows
            transaction.Commit()
            Ok(List.ofSeq affectedRowsByInsert)

        with error -> Error error

    let private populateCmd (cmd: SqliteCommand) (props: SqlProps) =
        if props.IsFunction
        then cmd.CommandType <- CommandType.StoredProcedure

        match props.Timeout with
        | Some timeout -> cmd.CommandTimeout <- timeout
        | None -> ()

        populateRow cmd props.Parameters

    let executeTransaction queries (props: SqlProps) =
        try
            if List.isEmpty queries then
                Ok []
            else
                let connection = getConnection props
                try
                    if not (connection.State.HasFlag ConnectionState.Open)
                    then connection.Open()
                    use transaction = connection.BeginTransaction()
                    let affectedRowsByQuery = ResizeArray<int>()
                    for (query, parameterSets) in queries do
                        if List.isEmpty parameterSets then
                            use command =
                                new SqliteCommand(query, connection, transaction)

                            let affectedRows = command.ExecuteNonQuery()
                            affectedRowsByQuery.Add affectedRows
                        else
                            for parameterSet in parameterSets do
                                use command =
                                    new SqliteCommand(query, connection, transaction)

                                populateRow command parameterSet
                                let affectedRows = command.ExecuteNonQuery()
                                affectedRowsByQuery.Add affectedRows

                    transaction.Commit()
                    Ok(List.ofSeq affectedRowsByQuery)
                finally
                    if props.ExistingConnection.IsNone then connection.Dispose()

        with error -> Error error

    let executeTransactionAsync queries (props: SqlProps) =
        async {
            try
                let! token = Async.CancellationToken

                use mergedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, props.CancellationToken)

                let mergedToken = mergedTokenSource.Token
                if List.isEmpty queries then
                    return Ok []
                else
                    let connection = getConnection props
                    try
                        if not (connection.State.HasFlag ConnectionState.Open)
                        then do! Async.AwaitTask(connection.OpenAsync mergedToken)
                        use transaction = connection.BeginTransaction()
                        let affectedRowsByQuery = ResizeArray<int>()
                        for (query, parameterSets) in queries do
                            if List.isEmpty parameterSets then
                                use command =
                                    new SqliteCommand(query, connection, transaction)

                                let! affectedRows = Async.AwaitTask(command.ExecuteNonQueryAsync mergedToken)
                                affectedRowsByQuery.Add affectedRows
                            else
                                for parameterSet in parameterSets do
                                    use command =
                                        new SqliteCommand(query, connection, transaction)

                                    populateRow command parameterSet
                                    let! affectedRows = Async.AwaitTask(command.ExecuteNonQueryAsync mergedToken)
                                    affectedRowsByQuery.Add affectedRows
                        transaction.Commit()
                        return Ok(List.ofSeq affectedRowsByQuery)
                    finally
                        if props.ExistingConnection.IsNone then connection.Dispose()
            with error -> return Error error
        }

    let execute (read: RowReader -> 't) (props: SqlProps): Result<'t list, exn> =
        try
            if props.SqlQuery.IsNone
            then failwith "No query provided to execute. Please use Sql.query"
            let connection = getConnection props
            try
                if not (connection.State.HasFlag ConnectionState.Open)
                then connection.Open()

                use command =
                    new SqliteCommand(props.SqlQuery.Value, connection)

                do populateCmd command props
                if props.NeedPrepare then command.Prepare()
                use reader = command.ExecuteReader()
                let rowReader = RowReader(reader)
                let result = ResizeArray<'t>()
                while reader.Read() do
                    result.Add(read rowReader)
                Ok(List.ofSeq result)
            finally
                if props.ExistingConnection.IsNone then connection.Dispose()
        with error -> Error error

    let iter (read: RowReader -> unit) (props: SqlProps): Result<unit, exn> =
        try
            if props.SqlQuery.IsNone
            then failwith "No query provided to execute. Please use Sql.query"
            let connection = getConnection props
            try
                if not (connection.State.HasFlag ConnectionState.Open)
                then connection.Open()

                use command =
                    new SqliteCommand(props.SqlQuery.Value, connection)

                do populateCmd command props
                if props.NeedPrepare then command.Prepare()
                use reader = command.ExecuteReader()
                let rowReader = RowReader(reader)
                while reader.Read() do
                    read rowReader
                Ok()
            finally
                if props.ExistingConnection.IsNone then connection.Dispose()
        with error -> Error error

    let iterAsync (read: RowReader -> unit) (props: SqlProps): Async<Result<unit, exn>> =
        async {
            try
                let! token = Async.CancellationToken

                use mergedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, props.CancellationToken)

                let mergedToken = mergedTokenSource.Token
                if props.SqlQuery.IsNone
                then failwith "No query provided to execute. Please use Sql.query"
                let connection = getConnection props
                try
                    if not (connection.State.HasFlag ConnectionState.Open)
                    then do! Async.AwaitTask(connection.OpenAsync(mergedToken))

                    use command =
                        new SqliteCommand(props.SqlQuery.Value, connection)

                    do populateCmd command props
                    if props.NeedPrepare then command.Prepare()
                    use! reader = Async.AwaitTask(command.ExecuteReaderAsync(mergedToken))
                    let rowReader = RowReader(reader)
                    while reader.Read() do
                        read rowReader
                    return Ok()
                finally
                    if props.ExistingConnection.IsNone then connection.Dispose()
            with error -> return Error error
        }

    let executeAsync (read: RowReader -> 't) (props: SqlProps): Async<Result<'t list, exn>> =
        async {
            try
                let! token = Async.CancellationToken

                use mergedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, props.CancellationToken)

                let mergedToken = mergedTokenSource.Token
                if props.SqlQuery.IsNone
                then failwith "No query provided to execute. Please use Sql.query"
                let connection = getConnection props
                try
                    if not (connection.State.HasFlag ConnectionState.Open)
                    then do! Async.AwaitTask(connection.OpenAsync(mergedToken))

                    use command =
                        new SqliteCommand(props.SqlQuery.Value, connection)

                    do populateCmd command props
                    if props.NeedPrepare then command.Prepare()
                    use! reader = Async.AwaitTask(command.ExecuteReaderAsync(mergedToken))
                    let rowReader = RowReader(reader)
                    let result = ResizeArray<'t>()
                    while reader.Read() do
                        result.Add(read rowReader)
                    return Ok(List.ofSeq result)
                finally
                    if props.ExistingConnection.IsNone then connection.Dispose()
            with error -> return Error error
        }

    /// Executes the query and returns the number of rows affected
    let executeNonQuery (props: SqlProps): Result<int, exn> =
        try
            if props.SqlQuery.IsNone
            then failwith "No query provided to execute..."
            let connection = getConnection props
            try
                if not (connection.State.HasFlag ConnectionState.Open)
                then connection.Open()

                use command =
                    new SqliteCommand(props.SqlQuery.Value, connection)

                populateCmd command props
                if props.NeedPrepare then command.Prepare()
                Ok(command.ExecuteNonQuery())
            finally
                if props.ExistingConnection.IsNone then connection.Dispose()
        with error -> Error error
    /// Executes the command and returns the number of rows affected
    let executeCommand (props: SqlProps): Result<int, exn> =
        try
            if props.SqlCommand.IsNone
            then failwith "No command provided to execute..."
            let connection = getConnection props
            try
                if not (connection.State.HasFlag ConnectionState.Open)
                then connection.Open()

                use command =
                    new SqliteCommand(props.SqlCommand.Value, connection)

                populateCmd command props
                if props.NeedPrepare then command.Prepare()
                Ok(command.ExecuteNonQuery())
            finally
                if props.ExistingConnection.IsNone then connection.Dispose()
        with error -> Error error

    /// Executes the query as asynchronously and returns the number of rows affected
    let executeNonQueryAsync (props: SqlProps) =
        async {
            try
                let! token = Async.CancellationToken

                use mergedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, props.CancellationToken)

                let mergedToken = mergedTokenSource.Token
                if props.SqlQuery.IsNone
                then failwith "No query provided to execute. Please use Sql.query"
                let connection = getConnection props
                try
                    if not (connection.State.HasFlag ConnectionState.Open)
                    then do! Async.AwaitTask(connection.OpenAsync(mergedToken))

                    use command =
                        new SqliteCommand(props.SqlQuery.Value, connection)

                    populateCmd command props
                    if props.NeedPrepare then command.Prepare()
                    let! affectedRows = Async.AwaitTask(command.ExecuteNonQueryAsync(mergedToken))
                    return Ok affectedRows
                finally
                    if props.ExistingConnection.IsNone then connection.Dispose()
            with error -> return Error error
        }
    open GenericDeconstructor
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
