module Fumble

open Feliz
open Feliz.Bulma
open Shared
let overview =
    Html.div [
        Bulma.title.h1 [
            Html.text "Fumble - Overview"
            Html.a [
                prop.href "https://www.nuget.org/packages/Fumble/"
                prop.children [
                    Html.img [
                        prop.src "https://img.shields.io/nuget/v/Fumble.svg?style=flat"
                    ]
                ]
            ]
        ]
        Bulma.subtitle.h2 [
            Html.text "Thin F# API for Sqlite for easy data access to sqlite database with functional seasoning on top"
        ]
        Html.hr []
        Bulma.title.h2 "Installation"
        Html.hr []
        Bulma.content [
            code "dotnet add package Fumble"
            code ".paket/paket.exe add Fumble --project path/to/project.fsproj"
        ]
        fixDocsView "Fumble"
    ]

