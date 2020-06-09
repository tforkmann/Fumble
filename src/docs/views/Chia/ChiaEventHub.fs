module ChiaEventHub

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.EventHub" ]
          Bulma.subtitle.h2
              [ Html.a
                  [ prop.text "Helper for Azure Event Hub" ]
                Html.text " extension for Feliz.Bulma" ]
          Html.hr []
          Bulma.content
              [ Html.p "You can use Chia to sent out a event to Azure Event Hubs like this:"
                code """
                open Chia.EventHubs

                let eventHubClient = getEventHubClient "EventHubSASConnectionString"

                type Data = int

                let data = 100

                do! pushEvent (eventHubClient,data,fileWriterInfoAzure)
                do! pushSingleEvent (eventHubClient,data,fileWriterInfoAzure)""" ] ]
