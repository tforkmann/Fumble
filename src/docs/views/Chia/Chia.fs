module Chia

open Feliz
open Feliz.Bulma

let overview =
    Html.div [
        Bulma.title.h1 [
            Html.text "Chia - Docs are WIP"
            Html.a [
                prop.href "https://www.nuget.org/packages/Chia/"
                prop.children [
                    Html.img [
                        prop.src "https://img.shields.io/nuget/v/Chia.svg?style=flat"
                    ]
                ]
            ]
        ]
        Bulma.subtitle.h2 [
            Html.text "Server - HelperFunctions for reporting"
        ]
        Html.hr []
        Bulma.content [
            Html.p "Chia contains HelperFunctions for reporting."
            Html.p "Chia contains some Azure Storage functions, logging features and some excel utils."
        ]
        Bulma.content [
            Bulma.title.h4 "Features"
            Html.ul [
                Html.li "Azure Helper Functions"
                Html.li "Setup your Azure infrastructure by using Farmer"
                Html.li "Local Logging features"
                Html.li "Application Insights tracing and event helper"
            ]
        ]
    ]

let installation = Shared.installationView "Chia"
