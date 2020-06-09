module ChiaTableStorage

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.TableStorage" ]
          Bulma.subtitle.h2
              [ Html.text "Helper to save data to Azure tables" ]
          Html.hr []
          Bulma.content
              [ Html.p "Save single entity"
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
