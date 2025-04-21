module Docs.Pages.JustifyContent

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
open Docs.SharedView


let justifyContentStyles = [
    justifyContent.justifyStart
    justifyContent.justifyCenter
    justifyContent.justifyEnd
    justifyContent.justifyBetween
    justifyContent.justifyAround
    justifyContent.justifyEvenly
]

let str prop =
    if prop = justifyContent.justifyStart then "justifyContent.justifyStart"
    elif prop = justifyContent.justifyCenter then "justifyContent.justifyCenter"
    elif prop = justifyContent.justifyEnd then "justifyContent.justifyEnd"
    elif prop = justifyContent.justifyBetween then "justifyContent.justifyBetween"
    elif prop = justifyContent.justifyAround then "justifyContent.justifyAround"
    elif prop = justifyContent.justifyEvenly then "justifyContent.justifyEvenly"
    else failwith "Unknown JustifyContent property"

let renderJustifyContent justifyContent =
    let example =
        Tailwind.divCombine [
            justifyContent
            prop.children [
                Tailwind.divCombine [
                    prop.text "1"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "2"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "3"
                    prop.className "w-6 h-6 bg-gray-300" ]
            ]
        ]

    let code =
        $"""Tailwind.divCombine [
            {str justifyContent}
            prop.children [
                Tailwind.divCombine [
                    prop.text "1"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "2"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "3"
                    prop.className "w-6 h-6 bg-gray-300" ]
            ]
        ]"""

    let title =
        Html.text
            "Use of JustifyContent is straightforward. Just add the JustifyContent property to the element you want to justify. Here are some examples:"
    codedView title code example

[<ReactComponent>]
let JustifyContentView () =
    React.fragment [
        for justifyContent in justifyContentStyles do
            renderJustifyContent justifyContent
    ]