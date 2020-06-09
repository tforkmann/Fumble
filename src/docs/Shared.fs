module Shared

open Fable.Core
open Fable.Core.JsInterop
open Feliz
open Feliz.Bulma

type Highlight =
    static member inline highlight (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-highlight", createObj !!properties)

let code (c:string) =
    Highlight.highlight [
        prop.className "fsharp"
        prop.text c
    ]


let installationView packageName =
    Html.div [
        Bulma.title.h1 (sprintf "%s - Installation" packageName)
        Html.hr []
        Bulma.content [
            Bulma.title.h4 "Use Paket"
            code (sprintf "paket add %s" packageName)
        ]
    ]
