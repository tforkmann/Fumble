module ChiaCreateBlob

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.CreateBlob" ]
          Bulma.subtitle.h2 [ Html.text "Helper to create a Azure blobs" ]
          Html.hr []
          Bulma.content
              [ Html.p "First create your blob container"
                code """
                open Chia.CreateBlob
                let containerInfo = {   StorageConnString = StorageConnString = StorageAccount.storageConnString
                                        ContainerName = "ContainerName"}
                let myContainer = getContainer containerInfo"""
                Html.p "Now you can get a list of all you blobs in the container like this:"
                code """
                let blobItems = getBlobs myContainero""" ] ]
