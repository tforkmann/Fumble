module Docs.Pages.JustifyItems

open Feliz
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI
open Feliz.Tailwind
open Feliz.Tailwind.Operators
open Docs.SharedView


let justifyItemsStyles = [
    justifyItems.justifyItemsStart
    justifyItems.justifyItemsEnd
    justifyItems.justifyItemsCenter
    justifyItems.justifyItemsStretch
    justifyItems.justifyItemsBaseline
]


let str prop =
    if prop = justifyItems.justifyItemsStart then "justifyItems.justifyItemsStart"
    elif prop = justifyItems.justifyItemsEnd then "justifyItems.justifyItemsEnd"
    elif prop = justifyItems.justifyItemsCenter then "justifyItems.justifyItemsCenter"
    elif prop = justifyItems.justifyItemsStretch then "justifyItems.justifyItemsStretch"
    elif prop = justifyItems.justifyItemsBaseline then "justifyItems.justifyItemsBaseline"
    else failwith "Unknown JustifyItems property"
let renderJustifyItems justifyItems =
    let example =
        Tailwind.divCombine [
            justifyItems
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
            {str justifyItems}
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
            "Use of JustifyItems is rather straightforward. Just add the JustifyItems property to the element you want to align. Here are some examples:"
    codedView title code example

[<ReactComponent>]
let JustifyItemsView () =
    React.fragment [
        for justifyItems in justifyItemsStyles do
            renderJustifyItems justifyItems
    ]