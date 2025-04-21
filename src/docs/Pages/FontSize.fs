module Docs.Pages.FontSize

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
open Docs.SharedView

let fontSizeStyles = [
    fontSize.textXs
    fontSize.textSm
    fontSize.textBase
    fontSize.textLg
    fontSize.textXl
    fontSize.text2Xl
    fontSize.text3Xl
    fontSize.text4Xl
    fontSize.text5Xl
    fontSize.text6Xl
    fontSize.text7Xl
    fontSize.text8Xl
    fontSize.text9Xl
]

let str prop =
    if prop = fontSize.textXs then "text-xs"
    elif prop = fontSize.textSm then "text-sm"
    elif prop = fontSize.textBase then "text-base"
    elif prop = fontSize.textLg then "text-lg"
    elif prop = fontSize.textXl then "text-xl"
    elif prop = fontSize.text2Xl then "text-2xl"
    elif prop = fontSize.text3Xl then "text-3xl"
    elif prop = fontSize.text4Xl then "text-4xl"
    elif prop = fontSize.text5Xl then "text-5xl"
    elif prop = fontSize.text6Xl then "text-6xl"
    elif prop = fontSize.text7Xl then "text-7xl"
    elif prop = fontSize.text8Xl then "text-8xl"
    elif prop = fontSize.text9Xl then "text-9xl"
    else "unknown"

let renderFontSize fontSize =
    let example =
        Daisy.button.button [
            button.outline
            button.primary
            button.lg
            fontSize
            prop.text (str fontSize)
        ]

    let code =
        $"""Daisy.button.button
            [   button.outline
                button.primary
                button.lg
                {str fontSize}
                prop.text "This is {str fontSize}" ]"""

    let title = Html.text "Use of fontSize is rather straightforward."
    codedView title code example

[<ReactComponent>]
let FontSizeView () =
    React.fragment [
        for fontSize in fontSizeStyles do
            renderFontSize fontSize
    ]