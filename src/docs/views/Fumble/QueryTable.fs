module QueryTable

open Feliz
open Feliz.Bulma
open Shared
let overview =
    Html.div [
        Bulma.title.h1 "Fumble - QueryTable"
        Html.hr []
        Bulma.content [
            Bulma.title.h4 "Connect to your database"
            Html.p [ prop.dangerouslySetInnerHTML "Get the connection from the environment" ]
            code """
            open Fumble
            let connectionString() = Env.getVar "app_db"""
        ]
        Html.hr []
        Bulma.content [
            Bulma.title.h4 "Connect to your database"
            Html.p [ prop.dangerouslySetInnerHTML "Get the connection from the environment" ]
            code """
            open Fumble
            let connectionString() = Env.getVar "app_db"""
        ]
    ]


// type User = { Id: int; Username: string }

// let getUsers() : Result<User list, exn> =
//     connectionString()
//     |> Sqlite.connect
//     |> Sqlite.query "SELECT * FROM dbo.[Users]"
//     |> Sqlite.execute (fun read ->
//         {
//             Id = read.int "user_id"
//             Username = read.string "username"
//         })
// ```
