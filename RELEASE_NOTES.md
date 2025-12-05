#### 2.0.1 - 2025-12-05
* **BREAKING**: Changed CREATE TABLE type mappings to use proper SQLite types (TEXT, INTEGER, REAL, BLOB)
* Add TimeSpan support (stored as ticks in INTEGER)
* Add DateOnly support (stored as TEXT yyyy-MM-dd)
* Add TimeOnly support (stored as ticks in INTEGER)
* Add byte, uint32, uint64 parameter and reader support
* Add executeScalar and executeScalarAsync for single value queries
* Add executeCount helper for COUNT queries
* Add executeExists helper for existence checks
* Add pagination helpers: skip, take, paginate
* Add SQLite-specific helpers: connectInMemory, connectInMemoryShared
* Add SQLite pragma helpers: pragma, pragmaSet, enableForeignKeys, enableWalMode, etc.
* Add database introspection: listTables, listIndexes, tableInfo, sqliteVersion
* Add maintenance functions: vacuum, analyze
* Add bulk operations: bulkUpdate, bulkDelete, upsert

#### 1.2.0 - 2025-04-18
*  Support DateTime options by default
#### 1.1.0 - 2024-12-11
*  Support for NET9
#### 1.0.3 - 2024-03-18
*  Remove SqlClient dependency
#### 1.0.2 - 2024-01-12
*  Update to net8.0
#### 1.0.1 - 2023-05-29
*  Renaming Sqlite to Sql to align with with Npgsql.FSharp
#### 1.0.0 - 2023-05-28
*  Renaming Sqlite to Sql to align with with Npgsql.FSharp
#### 0.9.0 - 2022-11-08
*  Update to .NET7
#### 0.8.3 - 2022-10-17
*  Update packages
#### 0.8.2 - 2022-06-07
*  Update packages and add float, float 32 and double
#### 0.8.1 - 2022-04-07
*  Update packages
#### 0.8.0 - 2022-02-17
*  Upgrade to net6.0
#### 0.7.9 - 2021-12-11
* try again
#### 0.7.8 - 2021-12-11
* try again
#### 0.7.7 - 2021-12-11
* Update packages and use new build style
#### 0.7.6 - 2021-02-16
* get rid of printouts
#### 0.7.5 - 2021-01-13
* update packages
#### 0.7.4 - 2020-12-04
* try again
#### 0.7.3 - 2020-12-04
* Add commandCreate command
#### 0.7.2 - 2020-12-01
* Add commandInsert command
#### 0.7.1 - 2020-11-13
* Update packages
#### 0.7.0 - 2020-11-13
* Update packages
#### 0.6.6 - 2020-11-05
* Clean up messages
#### 0.6.5 - 2020-11-04
* Fix db insert
#### 0.6.4 - 2020-10-29
* Fix for being able to insert Null values
#### 0.6.3 - 2020-10-28
* Update packages
#### 0.6.2 - 2020-10-16
* Upgrade to net5.0
#### 0.6.1 - 2020-10-16
* Upgrade to net5.0
#### 0.6.0 - 2020-10-16
* Upgrade to net5.0
#### 0.5.0 - 2020-10-05
* Update packages
#### 0.4.0 - 2020-07-26
* Update packages
#### 0.3.0 - 2020-07-26
* Add datetimeOffset
#### 0.2.0 - 2020-07-10
* Add insert function
#### 0.1.0 - 2020-06-09
* Initial release
