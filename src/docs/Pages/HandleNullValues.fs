module Docs.Pages.HandleNullValues

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
let HandleNullValuesView () =
    React.fragment [
        Html.divClassed "description" [ Html.text "Fumble - HandleNullValues" ]
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


// module HandleNullValues

// open Feliz
// open Feliz.Bulma
// open Shared
// let overview =
//     Html.div [
//         Bulma.title.h1 [
//             Html.text "Fumble - HandleNullValues"
//             Html.a [
//                 prop.href "https://www.nuget.org/packages/Fumble/"
//                 prop.children [
//                     Html.img [
//                         prop.src "https://img.shields.io/nuget/v/Fumble.svg?style=flat"
//                     ]
//                 ]
//             ]
//         ]
//         Bulma.subtitle.h2 [
//             Html.text "Thin F# API for Sqlite for easy data access to sqlite database with functional seasoning on top"
//         ]
//         Html.hr []
//         Bulma.content [
//             Html.p "dotnet add package Fumble"
//             Html.p ".paket/paket.exe add Fumble --project path/to/project.fsproj"
//         ]
//         fixDocsView "HandleNullValues"

//     ]

