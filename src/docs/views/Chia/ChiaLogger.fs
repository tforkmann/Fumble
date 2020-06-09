module ChiaLogger

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.Logger" ]
          Bulma.subtitle.h2
              [ Html.text "Helper for local logging or tracing for Azure Application Insights" ]
          Html.hr []
          Bulma.content
              [ Html.p "There are three log functions. `logStarting`, `logFinished` and `logCritical`."

                Html.p "The log function is using categories for clustering events in ApplicationInsights."
                Html.p "This will help you to get the most out of the ApplicationInsight dashboard and LogAnalytics."
                Html.p "If you want to log a information that a process is starting you can use `logStarting` like this:"
                code """
                Log.logStarting("Starting to get Data",LocalServer,Get,AzureTable,fileWriterInfo)""" ]
          Html.hr []
          Bulma.content
              [ Html.p "If a process finished as expected use `logFinished`:"
                code """
                Log.logFinished("Finisehd receiving Data",LocalServer,Get,AzureTable,fileWriterInfo)""" ]
          Html.hr []
          Bulma.content
              [ Html.p "If a process crashed unexpected use can track the error message with `logCritical`:"
                code """
                try
                    let trySomething = unsafe ()
                with
                | exn ->
                    let msg = sprintf  "Your error message: %s" exn.Message
                    Log.logCritical (msg,LocalService,LocalServer,Get,AzureTable,exn,fileWriterInfo)
                    failwith msg""" ]
          Html.hr []
          Bulma.content
              [ Html.p "Desciminated Unions"
                code """
                    type Source =
                        | LocalService
                        | LocalServer
                        | AzureFunction
                        | AzureInfrastucture
                        | PiServer
                        | Client
                        | SPSCommunication

                        type Operation =
                        | Upload
                        | Download
                        | Insert
                        | Query
                        | Create
                        | Delete
                        | Calculation
                        | Post
                        | Get
                        | Restart
                        | Stop

                        type Destination =
                        | AzureTable
                        | QueueTable
                        | BlobTable
                        | SqlTable
                        | LocalStorage
                        | EventHub
                    """ ]       ]
