module ChiaAiUtils

open Feliz
open Feliz.Bulma
open Shared
let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.AiUtils " ]
          Bulma.subtitle.h2
              [ Html.text "Helper for ApplicationInsights" ]
          Html.hr []
          Bulma.content
              [ Html.p "This library extends Feliz.Bulma by adding QuickView component"
                code """
                open Feliz.Bulma.QuickView
                QuickView.quickview [
                    if model.ShowQuickView then yield quickview.isActive
                    yield prop.children [
                        QuickView.header [
                            Html.div "Header"
                            Bulma.delete [ prop.onClick (fun _ -> ToggleQuickView |> dispatch) ]
                        ]
                        QuickView.body [
                            QuickView.block "Bulma is great"
                        ]
                        QuickView.footer [
                            Bulma.button "Save"
                        ]
                    ]
                ]""" ] ]
