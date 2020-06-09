module ChiaInfrastructure

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.Infrastructure" ]
          Bulma.subtitle.h2
              [ Html.text "Setup your Azure infrastructure by using Chia on top of "
                Html.a
                  [ prop.href "https://github.com/CompositionalIT/farmer/tree/master/src"
                    prop.text "Farmer" ]
                ]
          Html.hr []
          Bulma.content
              [ Html.p " You simply create a new FileWriter and then Chia will create a fresh Azure storageaccount in your prefered location."
                Html.p " Chia will connect to your storage account and you don't need to add you storage account at all."
                code """
                open Chia.Infrastructure
                open Chia.FileWriter
                open Chia.Domain.Config
                open Chia.Domain.Logging
                open Farmer
                let devStatus = Development
                let fileWriterInfo = initFileWriter devStatus "dp" "TestChia" Local ""
                let azAccount = azConnection fileWriterInfo Location.WestEurope""" ] ]
