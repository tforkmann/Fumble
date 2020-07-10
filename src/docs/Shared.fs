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


let fixDocsView fileName =
    Html.div [
        Html.a [
            prop.href (sprintf "https://github.com/tforkmann/Fumble/blob/master/src/docs/views/Fumble/%s.fs" fileName)
            prop.text ("Fix docs file " + fileName + " here")
        ]
    ]
