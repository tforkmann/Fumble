module HandleNullValues

open Feliz
open Feliz.Bulma

let overview =
    Html.div [
        Bulma.title.h1 [
            Html.text "Fumble - HandleNullValues"
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
        Bulma.content [
            Html.p "dotnet add package Fumble"
            Html.p ".paket/paket.exe add Fumble --project path/to/project.fsproj"
        ]
    ]

let installation = Shared.installationView "Fumble"
