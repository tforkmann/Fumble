module View

open Domain
open Feliz
open Feliz.Bulma
open Router
open Chia.Client.PageFlexer

let menuPart model dispatch =
    let item (t: string) p =
        let isActive =
            if model.CurrentPage = p then
                [ helpers.isActive
                   ]
            else
                []

        Bulma.menuItem.a
            [ yield! isActive
              prop.onClick (fun _ -> (SentToast t) |> dispatch)
              prop.text t
              prop.href (getHref p) ]

    Bulma.menu
        [ Bulma.menuLabel "Chia"
          Bulma.menuList [
              item "Overview" Chia ]
          Bulma.menuList
              [ item "Installation" ChiaInstallation
                item "FileWriter" ChiaFileWriter
                item "Infrastructure" ChiaInfrastructure
                item "Logger" ChiaLogger
                item "CreateTable" ChiaCreateTable
                item "CreateBlob" ChiaCreateBlob
                item "CreateJsonBlob" ChiaCreateJsonBlob
                item "CreateXml" ChiaCreateXml
                item "PostToQueue" ChiaPostToQueue
                item "GetTableEntry" ChiaGetTableEntry
                item "RedisCache" ChiaRedisCache
                item "EventHub" ChiaEventHub
                item "TableStorage" ChiaTableStorage
                item "AiUtils" ChiaAiUtils
                item "ExcelUtils" ChiaExcelUtils ]
          Bulma.menuLabel "Chia.Client"
          Bulma.menuList
              [ item "Overview" ChiaClient
                item "Installation" ChiaClientInstallation
                item "PageFlexer" ChiaClientPageFlexer ] ]

let contentPart model dispatch =
    match model.CurrentPage with
    | Chia -> Chia.overview
    | ChiaInstallation -> Chia.installation
    | ChiaFileWriter -> ChiaFileWriter.overview
    | ChiaAiUtils -> ChiaAiUtils.overview
    | ChiaRedisCache -> ChiaRedisCache.overview
    | ChiaEventHub -> ChiaEventHub.overview
    | ChiaCreateXml -> ChiaCreateXml.overview
    | ChiaCreateTable -> ChiaCreateTable.overview
    | ChiaCreateBlob -> ChiaCreateBlob.overview
    | ChiaCreateJsonBlob -> ChiaCreateJsonBlob.overview
    | ChiaPostToQueue -> ChiaPostToQueue.overview
    | ChiaGetTableEntry -> ChiaGetTableEntry.overview
    | ChiaLogger -> ChiaLogger.overview
    | ChiaInfrastructure -> ChiaInfrastructure.overview
    | ChiaExcelUtils -> ChiaExcelUtils.overview
    | ChiaTableStorage -> ChiaTableStorage.overview
    | ChiaClient -> ChiaClient.overview
    | ChiaClientInstallation -> ChiaClient.installation
    | ChiaClientPageFlexer -> ChiaPageFlexer.overview
// | BulmaInstallation -> Views.Bulma.installation
// | BulmaAPIDescription -> Views.Bulma.apiDescription
// | QuickViewOverview -> Views.QuickView.overview model dispatch
// | QuickViewInstallation -> Views.QuickView.installation
// | CalendarOverview -> Views.Calendar.overview
// | CalendarInstallation -> Views.Calendar.installation
// | TooltipOverview -> Views.Tooltip.overview
// | TooltipInstallation -> Views.Tooltip.installation
// | CheckradioOverview -> Views.Checkradio.overview
// | CheckradioInstallation -> Views.Checkradio.installation
// | SwitchOverview -> Views.Switch.overview
// | SwitchInstallation -> Views.Switch.installation
// | PopoverOverview -> Views.Popover.overview
// | PopoverInstallation -> Views.Popover.installation
// | PageLoaderOverview -> Views.PageLoader.overview model dispatch
// | PageLoaderInstallation -> Views.PageLoader.installation

let view (model: Model) (dispatch: Msg -> unit) =

    let render =
        Bulma.container
            [ Bulma.section
                [ Bulma.tile
                    [ tile.isAncestor
                      prop.children
                          [ Bulma.tile
                              [ tile.is2
                                prop.children (menuPart model dispatch) ]
                            Bulma.tile (contentPart model dispatch) ] ] ] ]

    Router.router
        [ Router.onUrlChanged (parseUrl >> UrlChanged >> dispatch)
          Router.application render ]
