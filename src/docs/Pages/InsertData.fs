module Docs.Pages.InsertData

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
let InsertDataView () =
    React.fragment [
        Html.divClassed "description" [ Html.text "Fumble - InsertData" ]
        Html.divClassed "max-w-xl" [ linedMockupCode "Connect to your database" ]
        Html.divClassed "description" [
            Html.text "Get the connection from the environment"
            Html.code [
                prop.className "code"
                prop.text """
                    open Fumble
                    let connectionString() = Env.getVar "app_d" """
            ]
        ]
        Html.divClassed "max-w-xl" [ linedMockupCode "Insert trade data into database with commandInsert" ]
        Html.divClassed "description" [
            Html.code [
                prop.className "code"
                prop.text """
                    connectionString()
                    |> Sqlite.connect
                    |> Sqlite.commandInsert<Trades>("Trades,trades)
                    |> Sqlite.insertData trades
                    |> function
                    | Ok x ->
                        printfn "rows affected %A" x.Length
                        pass ()
                    | otherwise ->
                        printfn "error %A" otherwise
                        fail () """
            ]
        ]
        fixDocsView "InsertData"
    ]
