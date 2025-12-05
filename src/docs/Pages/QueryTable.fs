module Docs.Pages.QueryTable

open Feliz
open Feliz.DaisyUI
open Feliz.Tailwind
open Docs.SharedView

let boxShadowStyles =
    [ boxShadow.shadow2Xs
      boxShadow.shadowXs
      boxShadow.shadowSm
      boxShadow.shadowMd
      boxShadow.shadowLg
      boxShadow.shadowXl
      boxShadow.shadow2Xl
      boxShadow.shadowInner
      boxShadow.shadowNone ]

let str prop =
    if prop = boxShadow.shadow2Xs then
        "boxShadow.shadow2Xs"
    elif prop = boxShadow.shadowXs then
        "boxShadow.shadowXs"
    elif prop = boxShadow.shadowSm then
        "boxShadow.shadowSm"
    elif prop = boxShadow.shadowMd then
        "boxShadow.shadowMd"
    elif prop = boxShadow.shadowLg then
        "boxShadow.shadowLg"
    elif prop = boxShadow.shadowXl then
        "boxShadow.shadowXl"
    elif prop = boxShadow.shadow2Xl then
        "boxShadow.shadow2Xl"
    elif prop = boxShadow.shadowInner then
        "boxShadow.shadowInner"
    elif prop = boxShadow.shadowNone then
        "boxShadow.shadowNone"
    else
        failwith "Unknown BoxShadow property"
let renderBoxShadow boxShadow =
    let example =
        Daisy.button.button
            [   button.outline
                button.primary
                button.lg
                boxShadow
                prop.text (str boxShadow) ]

    let code =
        $"""Daisy.button.button
                [   button.outline
                    button.primary
                    button.lg
                    {str boxShadow}
                    prop.text "This is {str boxShadow}" ]"""

    let title = Html.text "Use of borderRadius is rather straightforward."
    codedView title code example

[<ReactComponent>]
let QueryTableView () =
    React.fragment [
        Html.divClassed "description" [ Html.text "Fumble - QueryTable" ]
        Html.divClassed "max-w-xl" [ linedMockupCode "open Fumble" ]
        Html.divClassed "description" [
            Html.text "Now you can start using library. Everything important starts with "
            Html.code [
                prop.className "code"
                prop.text """
                    open Fumble
                    let connectionString() = Env.getVar "app_d" """
            ]
        ]
    ]


// QueryTable

// open Feliz
// open Feliz.Bulma
// open Shared
// let overview =
//     Html.div [
//         Bulma.title.h1 "Fumble - QueryTable"
//         Html.hr []
//         Bulma.content [
//             Bulma.title.h4 "Connect to your database"
//             Html.p [ prop.dangerouslySetInnerHTML "Get the connection from the environment" ]
//             code """
//             open Fumble
//             let connectionString() = Env.getVar "app_db"""
//         ]
//         Html.hr []
//         Bulma.content [
//             Bulma.title.h4 "Query data from to your database"
//             code """
//             let data =
//                 connectionString()
//                 |> Sqlite.connect
//                 |> Sqlite.query
//                 "
//                 SELECT * FROM Trades
//                 ORDER BY timestamp desc
//                 "
//                 |> Sqlite.execute (fun read ->
//                     { Symbol = read.string "Symbol"
//                       Timestamp = read.dateTime "Timestamp"
//                       Price = read.double "Price"
//                       TradeSize = read.double "TradeSize" })
//                 |> function
//                 | Ok x -> x
//                 | otherwise ->
//                     printfn "error %A" otherwise
//                     fail () """
//         ]
//         fixDocsView "QueryTable"

//     ]


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
