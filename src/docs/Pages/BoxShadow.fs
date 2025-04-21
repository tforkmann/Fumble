module Docs.Pages.BoxShadow

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
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
let BoxShadowView () =
    React.fragment [
        for boxShadow in boxShadowStyles do
            renderBoxShadow boxShadow
    ]