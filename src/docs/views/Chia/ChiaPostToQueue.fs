module ChiaPostToQueue

open Feliz
open Feliz.Bulma

open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.PostToQueue" ]
          Bulma.subtitle.h2
              [ Html.text "Helper for Azure Queue messages" ]
          Html.hr []
          Bulma.content
              [ Html.p "You can use Chia to sent out a Azure Queue message like this:"
                code """
                open Chia.PostToQueue
                open Chia.CreateTable


                let connected =
                    let connection = AzureConnection StorageAccount.storageConnString
                    connection.Connect()

                [<Literal>]
                let SendMail = "sendmail-queue"

                let sendQueue = getQueue connected SendMail fileWriterInfo""" ] ]
