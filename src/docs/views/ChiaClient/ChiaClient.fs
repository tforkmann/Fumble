module ChiaClient

open Feliz
open Feliz.Bulma

let overview =
    Html.div [
        Bulma.title.h1 [
            Html.text "Chia.Client"
            Html.a [
                prop.href "https://www.nuget.org/packages/Chia.Client/"
                prop.children [
                    Html.img [
                        prop.src "https://img.shields.io/nuget/v/Chia.Client.svg?style=flat"
                    ]
                ]
            ]
        ]
        Bulma.subtitle.h2 [
            Html.text "Client - HelperFunctions for reporting"
        ]
        Html.hr []
        Bulma.content [
            Html.p "Chia.Client contains HelperFunctions to help with Fable visualizations. For example PageFlexer, ExcelExport Helper etc."
        ]
        Bulma.content [
            Bulma.title.h4 "Features"
            Html.ul [
                Html.li "PageFlexer"
                Html.li "ExcelExport Helper"
                Html.li "Toast Messages"
                Html.li "TimeCalculation"
                Html.li "TimeModelHelper"
                Html.li "TableHelper"
            ]
        ]
    ]

let installation = Shared.installationView "Chia.Client"


let fileWriter =
    Html.div [
        Bulma.title.h1 [
            Html.text "Chia - FileWriter"
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
            Html.text " HelperFunctions for reporting"
        ]
        Html.hr []
        Bulma.content [
            Html.p "Chia contains HelperFunctions for reporting.. Instead of writing:"
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
