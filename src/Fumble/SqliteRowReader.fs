namespace Fumble

open System
open System.Collections.Generic
open Microsoft.Data.Sqlite

type RowReader(reader: SqliteDataReader) =
    let columnDict = Dictionary<string, int>()
    let columnTypes = Dictionary<string, string>()
    let types = ResizeArray<string>()

    do
        // Populate the names of the columns into a dictionary
        // such that each read doesn't need to loop through all columns
        for fieldIndex in [ 0 .. reader.FieldCount - 1 ] do
            let columnName = reader.GetName(fieldIndex)
            let columnType = reader.GetDataTypeName(fieldIndex)
            columnDict.Add(columnName, fieldIndex)
            columnTypes.Add(columnName, columnType)
            types.Add(columnType)

    let failToRead (column: string) (columnType: string) =
        let availableColumns =
            columnDict.Keys
            |> Seq.map (fun key -> sprintf "[%s:%s]" key columnTypes.[key])
            |> String.concat ", "

        failwithf "Could not read column '%s' as %s. Available columns are %s" column columnType availableColumns
    with

        member __.int(columnIndex: int): int =
            if types.[columnIndex] = "tinyint" then int (reader.GetByte columnIndex)
            elif types.[columnIndex] = "smallint" then int (reader.GetInt16 columnIndex)
            else reader.GetInt32 columnIndex

        member __.int(column: string): int =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> __.int columnIndex
            | false, _ -> failToRead column "int"

        member __.intOrNone(column: string): int option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(__.int columnIndex)
            | false, _ -> failToRead column "int"

        member __.intOrNone(columnIndex: int) =
            if reader.IsDBNull(columnIndex) then None else Some(__.int (columnIndex))

        member __.tinyint(column: string) =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetByte(columnIndex)
            | false, _ -> failToRead column "tinyint"

        member __.tinyintOrNone(column: string) =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetByte(columnIndex))
            | false, _ -> failToRead column "tinyint"

        member __.int16(columnIndex: int) =
            if types.[columnIndex] = "tinyint" then
                int16 (reader.GetByte(columnIndex))
            else
                reader.GetInt16(columnIndex)

        member __.int16(column: string): int16 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> __.int16 (columnIndex)
            | false, _ -> failToRead column "int16"

        member __.int16OrNone(column: string): int16 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(__.int16 (columnIndex))
            | false, _ -> failToRead column "int16"

        member __.int16OrNone(columnIndex: int) =
            if reader.IsDBNull(columnIndex) then None else Some(__.int16 (columnIndex))

        member __.int64(columnIndex: int): int64 =
            if types.[columnIndex] = "tinyint"
               || types.[columnIndex] = "smallint"
               || types.[columnIndex] = "int" then
                Convert.ToInt64(reader.GetValue(columnIndex))
            else
                reader.GetInt64(columnIndex)

        member __.int64(column: string): int64 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> __.int64 (columnIndex)
            | false, _ -> failToRead column "int64"

        member __.int64OrNone(column: string): int64 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(__.int64 columnIndex)
            | false, _ -> failToRead column "int64"

        member __.int64OrNone(columnIndex: int): int64 option =
            if reader.IsDBNull(columnIndex) then None else Some(__.int64 (columnIndex))

        member __.string(column: string): string =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if types.[columnIndex] = "uniqueidentifier" then
                    string (reader.GetGuid(columnIndex))
                else
                    reader.GetString(columnIndex)
            | false, _ -> failToRead column "string"

        member __.stringOrNone(column: string): string option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetString(columnIndex))
            | false, _ -> failToRead column "string"

        member __.bool(columnIndex: int) = reader.GetBoolean(columnIndex)

        member __.bool(column: string): bool =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetBoolean(columnIndex)
            | false, _ -> failToRead column "bool"

        member __.boolOrNone(column: string): bool option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetBoolean(columnIndex))
            | false, _ -> failToRead column "bool"

        member __.decimal(columnIndex: int) = reader.GetDecimal(columnIndex)

        member __.decimal(column: string): decimal =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                let columnType = types.[columnIndex]
                if List.contains columnType [ "int"; "int64"; "int16"; "float" ]
                then Convert.ToDecimal(reader.GetValue(columnIndex))
                else reader.GetDecimal(columnIndex)
            | false, _ -> failToRead column "decimal"

        member __.decimalOrNone(column: string): decimal option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDecimal(columnIndex))
            | false, _ -> failToRead column "decimal"

        member __.double(columnIndex: int) = reader.GetDouble(columnIndex)

        member __.double(column: string): double =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDouble(columnIndex)
            | false, _ -> failToRead column "double"

        member __.doubleOrNone(column: string): double option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDouble(columnIndex))
            | false, _ -> failToRead column "double"

        member __.Reader = reader

        member __.uniqueidentifier(columnIndex: int) = reader.GetGuid(columnIndex)

        /// Gets the value of the specified column as a globally-unique identifier (GUID).
        member __.uniqueidentifier(column: string): Guid =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetGuid(columnIndex)
            | false, _ -> failToRead column "uniqueidentifier"

        member __.uniqueidentifierOrNone(column: string): Guid option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetGuid(columnIndex))
            | false, _ -> failToRead column "uniqueidentifier"

        /// Gets the value of the specified column as a System.DateTime object.
        member __.dateTimeOffset(column: string): DateTimeOffset =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDateTimeOffset(columnIndex)
            | false, _ -> failToRead column "dateTimeOffset"

        /// Gets the value of the specified column as a System.DateTime object.
        member __.dateTimeOffsetOrNone(column: string): DateTimeOffset option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if reader.IsDBNull(columnIndex)
                then None
                else Some(reader.GetDateTimeOffset(columnIndex))
            | false, _ -> failToRead column "dateTimeOffset"

        member __.dateTime(columnIndex: int) = reader.GetDateTime(columnIndex)

        /// Gets the value of the specified column as a System.DateTime object.
        member __.dateTime(column: string): DateTime =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDateTime(columnIndex)
            | false, _ -> failToRead column "datetime"

        /// Gets the value of the specified column as a System.DateTime object.
        member __.dateTimeOrNone(column: string): DateTime option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDateTime(columnIndex))
            | false, _ -> failToRead column "datetime"

        member __.bytes(columnIndex: int) = reader.GetFieldValue<byte []>(columnIndex)

        /// Reads the specified column as `byte[]`
        member __.bytes(column: string): byte [] =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetFieldValue<byte []>(columnIndex)
            | false, _ -> failToRead column "byte[]"

        /// Reads the specified column as `byte[]`
        member __.bytesOrNone(column: string): byte [] option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if reader.IsDBNull(columnIndex)
                then None
                else Some(reader.GetFieldValue<byte []>(columnIndex))
            | false, _ -> failToRead column "byte[]"

        member __.float32(columnIndex: int) = reader.GetFloat(columnIndex)

        /// Gets the value of the specified column as a `System.Single` object.
        member __.float32(column: string): float32 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetFloat(columnIndex)
            | false, _ -> failToRead column "float32"

        /// Gets the value of the specified column as a `System.Single` object.
        member __.float32OrNone(column: string): float32 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetFloat(columnIndex))
            | false, _ -> failToRead column "float32"

        member __.float(columnIndex: int) = reader.GetDouble(columnIndex)

        /// Gets the value of the specified column as a `System.Float` object.
        member __.float(column: string): float =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDouble(columnIndex)
            | false, _ -> failToRead column "float"

        member __.floatOrNone(column: string): float option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDouble(columnIndex))
            | false, _ -> failToRead column "float"
