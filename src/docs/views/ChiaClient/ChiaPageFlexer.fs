module ChiaPageFlexer

open Feliz
open Feliz.Bulma
open Shared
open Chia.Client.PageFlexer

let content =
    Bulma.content
      [ Html.p "Use PageFlexer like this"
        code """
        pageFlexer [] [
            Html.div
                [ Bulma.title.h1 [ Html.text "Chia.Client.PageFlexer" ]
                  Bulma.subtitle.h2 ]]""" ]

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.Client.PageFlexer" ]
          Bulma.subtitle.h2
              [ Html.a
                  [ prop.href "https://wikiki.github.io/components/quickview/"
                    prop.text "QuickView" ]
                Html.text " extension for Feliz.Bulma" ]
          Html.hr []
          content ]
