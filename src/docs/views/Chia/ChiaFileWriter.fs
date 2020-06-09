module ChiaFileWriter

open Feliz
open Feliz.Bulma
open Shared



let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.FileWriter" ]
          Bulma.subtitle.h2
              [ Html.a
                  [ prop.href "https://wikiki.github.io/components/quickview/"
                    prop.text "QuickView" ]
                Html.text "Initialize your FileWriter instant with `initFileWriter`" ]
          Html.hr []
          Bulma.content
              [ Html.p "If you want to log to ApplicationInsight you additionally have to create a new Application Insight resource in Azure and set your ApplicationInsights key."
                code """
                open Chia.Domain.Logging
                open Chia.Domain.Config
                open Chia.FileWriter
                let devStatus = getDevStatusFromEnv  /// Get your devStatus from you enviroment variable. For example pass in an enviroment variable in Fake --> '-e devStatus=Productive
                let aiKey = "<InsertYourApplicationInsightsKey" ///Get this key from your app.config or from KeyFault
                let fileWriterInfo = initFileWriter devStatus "ProjectName" Local ""
                let fileWriterInfoAzure = initFileWriter devStatus "ProjectName" Azure aiKey
                let fileWriterInfoLocalAndAzure = initFileWriter devStatus "ProjectName" LocalAndAzure aiKey""" ] ]
