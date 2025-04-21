module Docs.Pages.TextAlign

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
open Docs.SharedView


let textAlignStyles = [
    textAlign.left
    textAlign.center
    textAlign.right
    textAlign.justify
    textAlign.start
    textAlign.``end``
]


let str prop =
    if prop = textAlign.left then "textAlign.left"
    elif prop = textAlign.center then "textAlign.center"
    elif prop = textAlign.right then "textAlign.right"
    elif prop = textAlign.justify then "textAlign.justify"
    elif prop = textAlign.start then "textAlign.start"
    elif prop = textAlign.``end`` then "textAlign.end"
    else failwith "Unknown TextAlign property"

let renderTextAlign textAlign =
    let example =
        Html.p [
            textAlign
            prop.text
                "So I started to walk into the water. I won't lie to you boys, I was terrified. But I pressed on, and as I made my way past the breakers a strange calm came over me. I don't know if it was divine intervention or the kinship of all living things but I tell you Jerry at that moment, I was a marine biologist."
        ]

    let code =
        $"""Html.p [
            {str textAlign}
            prop.text "So I started to walk into the water. I won't lie to you"""

    let title =
        Html.text
            "Use of TextAlign is straightforward. Just add the TextAlign property to the element you want to align. Here are some examples:"

    codedView title code example

[<ReactComponent>]
let TextAlignView () =
    React.fragment [
        for textAlign in textAlignStyles do
            renderTextAlign textAlign
    ]