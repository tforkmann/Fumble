module Docs.Pages.BorderRadius

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
open Docs.SharedView

let borderRadiusStyles = [
    borderRadius.roundedNone
    borderRadius.roundedSm
    borderRadius.roundedMd
    borderRadius.roundedLg
    borderRadius.roundedXl
    borderRadius.rounded2Xl
    borderRadius.rounded3Xl
]

let str prop =
    if prop = borderRadius.roundedNone then "rounded-none"
    elif prop = borderRadius.roundedSm then "rounded-sm"
    elif prop = borderRadius.roundedMd then "rounded-md"
    elif prop = borderRadius.roundedLg then "rounded-lg"
    elif prop = borderRadius.roundedXl then "rounded-xl"
    elif prop = borderRadius.rounded2Xl then "rounded-2xl"
    elif prop = borderRadius.rounded3Xl then "rounded-3xl"
    else "unknown"

let renderBorderRadius borderRadius =
    let example =
        Daisy.button.button [
            button.outline
            button.primary
            button.lg
            borderRadius
            prop.text (str borderRadius)
        ]

    let code =
        $"""Daisy.button.button
            [   button.outline
                button.primary
                button.lg
                {str borderRadius}
                prop.text "This is {str borderRadius}" ]"""

    let title = Html.text "Use of borderRadius is rather straightforward."
    codedView title code example

[<ReactComponent>]
let BorderRadiusView () =
    React.fragment [
        for borderRadius in borderRadiusStyles do
            renderBorderRadius borderRadius
    ]