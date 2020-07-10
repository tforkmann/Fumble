namespace Fumble

open System
open System.Collections.Generic
open Microsoft.Data.Sqlite

type SqliteRowReader(reader: SqliteDataReader) =
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
            columnTypes.Add(reader.GetName(fieldIndex), columnType)
            types.Add(columnType)

    let failToRead (column: string) (columnType: string) =
        let availableColumns =
            columnDict.Keys
            |> Seq.map (fun key -> sprintf "[%s:%s]" key columnTypes.[key])
            |> String.concat ", "

        failwithf "Could not read column '%s' as %s. Available columns are %s" column columnType availableColumns
    with

        member this.int(columnIndex: int): int =
            if types.[columnIndex] = "tinyint" then int (reader.GetByte columnIndex)
            elif types.[columnIndex] = "smallint" then int (reader.GetInt16 columnIndex)
            else reader.GetInt32 columnIndex

        member this.int(column: string): int =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> this.int columnIndex
            | false, _ -> failToRead column "int"

        member this.intOrNone(column: string): int option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(this.int columnIndex)
            | false, _ -> failToRead column "int"

        member this.intOrNone(columnIndex: int) =
            if reader.IsDBNull(columnIndex) then None else Some(this.int (columnIndex))

        member this.tinyint(column: string) =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetByte(columnIndex)
            | false, _ -> failToRead column "tinyint"

        member this.tinyintOrNone(column: string) =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetByte(columnIndex))
            | false, _ -> failToRead column "tinyint"

        member this.int16(columnIndex: int) =
            if types.[columnIndex] = "tinyint" then
                int16 (reader.GetByte(columnIndex))
            else
                reader.GetInt16(columnIndex)

        member this.int16(column: string): int16 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> this.int16 (columnIndex)
            | false, _ -> failToRead column "int16"

        member this.int16OrNone(column: string): int16 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(this.int16 (columnIndex))
            | false, _ -> failToRead column "int16"

        member this.int16OrNone(columnIndex: int) =
            if reader.IsDBNull(columnIndex) then None else Some(this.int16 (columnIndex))

        member this.int64(columnIndex: int): int64 =
            if types.[columnIndex] = "tinyint"
               || types.[columnIndex] = "smallint"
               || types.[columnIndex] = "int" then
                Convert.ToInt64(reader.GetValue(columnIndex))
            else
                reader.GetInt64(columnIndex)

        member this.int64(column: string): int64 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> this.int64 (columnIndex)
            | false, _ -> failToRead column "int64"

        member this.int64OrNone(column: string): int64 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(this.int64 columnIndex)
            | false, _ -> failToRead column "int64"

        member this.int64OrNone(columnIndex: int): int64 option =
            if reader.IsDBNull(columnIndex) then None else Some(this.int64 (columnIndex))

        member this.string(column: string): string =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if types.[columnIndex] = "uniqueidentifier" then
                    string (reader.GetGuid(columnIndex))
                else
                    reader.GetString(columnIndex)
            | false, _ -> failToRead column "string"

        member this.stringOrNone(column: string): string option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetString(columnIndex))
            | false, _ -> failToRead column "string"

        member this.bool(columnIndex: int) = reader.GetBoolean(columnIndex)

        member this.bool(column: string): bool =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetBoolean(columnIndex)
            | false, _ -> failToRead column "bool"

        member this.boolOrNone(column: string): bool option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetBoolean(columnIndex))
            | false, _ -> failToRead column "bool"

        member this.decimal(columnIndex: int) = reader.GetDecimal(columnIndex)

        member this.decimal(column: string): decimal =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                let columnType = types.[columnIndex]
                if List.contains columnType [ "int"; "int64"; "int16"; "float" ]
                then Convert.ToDecimal(reader.GetValue(columnIndex))
                else reader.GetDecimal(columnIndex)
            | false, _ -> failToRead column "decimal"

        member this.decimalOrNone(column: string): decimal option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDecimal(columnIndex))
            | false, _ -> failToRead column "decimal"

        member this.double(columnIndex: int) = reader.GetDouble(columnIndex)

        member this.double(column: string): double =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDouble(columnIndex)
            | false, _ -> failToRead column "double"

        member this.doubleOrNone(column: string): double option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDouble(columnIndex))
            | false, _ -> failToRead column "double"

        member this.Reader = reader

        member this.uniqueidentifier(columnIndex: int) = reader.GetGuid(columnIndex)

        /// Gets the value of the specified column as a globally-unique identifier (GUID).
        member this.uniqueidentifier(column: string): Guid =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetGuid(columnIndex)
            | false, _ -> failToRead column "uniqueidentifier"

        member this.uniqueidentifierOrNone(column: string): Guid option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetGuid(columnIndex))
            | false, _ -> failToRead column "uniqueidentifier"

        /// Gets the value of the specified column as a System.DateTime object.
        member this.dateTimeOffset(column: string): DateTimeOffset =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDateTimeOffset(columnIndex)
            | false, _ -> failToRead column "dateTimeOffset"

        /// Gets the value of the specified column as a System.DateTime object.
        member this.dateTimeOffsetOrNone(column: string): DateTimeOffset option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if reader.IsDBNull(columnIndex)
                then None
                else Some(reader.GetDateTimeOffset(columnIndex))
            | false, _ -> failToRead column "dateTimeOffset"

        member this.dateTime(columnIndex: int) = reader.GetDateTime(columnIndex)

        /// Gets the value of the specified column as a System.DateTime object.
        member this.dateTime(column: string): DateTime =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetDateTime(columnIndex)
            | false, _ -> failToRead column "datetime"

        /// Gets the value of the specified column as a System.DateTime object.
        member this.dateTimeOrNone(column: string): DateTime option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetDateTime(columnIndex))
            | false, _ -> failToRead column "datetime"

        member this.bytes(columnIndex: int) = reader.GetFieldValue<byte []>(columnIndex)

        /// Reads the specified column as `byte[]`
        member this.bytes(column: string): byte [] =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetFieldValue<byte []>(columnIndex)
            | false, _ -> failToRead column "byte[]"

        /// Reads the specified column as `byte[]`
        member this.bytesOrNone(column: string): byte [] option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex ->
                if reader.IsDBNull(columnIndex)
                then None
                else Some(reader.GetFieldValue<byte []>(columnIndex))
            | false, _ -> failToRead column "byte[]"

        member this.float(columnIndex: int) = reader.GetFloat(columnIndex)

        /// Gets the value of the specified column as a `System.Single` object.
        member this.float(column: string): float32 =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> reader.GetFloat(columnIndex)
            | false, _ -> failToRead column "float"

        /// Gets the value of the specified column as a `System.Single` object.
        member this.floatOrNone(column: string): float32 option =
            match columnDict.TryGetValue(column) with
            | true, columnIndex -> if reader.IsDBNull(columnIndex) then None else Some(reader.GetFloat(columnIndex))
            | false, _ -> failToRead column "float"
