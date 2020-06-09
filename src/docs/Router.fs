module Router

open Feliz.Router

type Page =
    | Chia
    | ChiaInstallation
    | ChiaFileWriter
    | ChiaAiUtils
    | ChiaRedisCache
    | ChiaEventHub
    | ChiaCreateXml
    | ChiaCreateTable
    | ChiaCreateBlob
    | ChiaCreateJsonBlob
    | ChiaPostToQueue
    | ChiaLogger
    | ChiaInfrastructure
    | ChiaGetTableEntry
    | ChiaExcelUtils
    | ChiaTableStorage
    | ChiaClient
    | ChiaClientInstallation
    | ChiaClientPageFlexer

    // | BulmaAPIDescription
    // | QuickViewOverview
    // | QuickViewInstallation
    // | CalendarOverview
    // | CalendarInstallation
    // | TooltipOverview
    // | TooltipInstallation
    // | CheckradioOverview
    // | CheckradioInstallation
    // | SwitchOverview
    // | SwitchInstallation
    // | PopoverOverview
    // | PopoverInstallation
    // | PageLoaderOverview
    // | PageLoaderInstallation

let defaultPage = Chia

let parseUrl = function
    | [ "" ] -> Chia
    | [ "installation" ] -> ChiaInstallation
    | [ "filewriter" ] -> ChiaFileWriter
    | [ "aiutils" ] -> ChiaAiUtils
    | [ "rediscache" ] -> ChiaRedisCache
    | [ "eventhub" ] -> ChiaEventHub
    | [ "createxml" ] -> ChiaCreateXml
    | [ "createtable" ] -> ChiaCreateTable
    | [ "createblob" ] -> ChiaCreateBlob
    | [ "createjsonblob" ] -> ChiaCreateJsonBlob
    | [ "posttoqueue" ] -> ChiaPostToQueue
    | [ "gettableentry" ] -> ChiaGetTableEntry
    | [ "logger" ] -> ChiaLogger
    | [ "infrastructure" ] -> ChiaInfrastructure
    | [ "excelutils" ] -> ChiaExcelUtils
    | [ "tablestorage" ] -> ChiaTableStorage
    | [ "client" ] -> ChiaClient
    | [ "clientinstallation" ] -> ChiaClientInstallation
    | [ "clientpageflexer" ] -> ChiaClientPageFlexer


    // | [ "installation" ] -> BulmaInstallation
    // | [ "api-description" ] -> BulmaAPIDescription
    // | [ "quickview"; "installation" ] -> QuickViewInstallation
    // | [ "quickview" ] -> QuickViewOverview
    // | [ "calendar"; "installation" ] -> CalendarInstallation
    // | [ "calendar" ] -> CalendarOverview
    // | [ "tooltip"; "installation" ] -> TooltipInstallation
    // | [ "tooltip" ] -> TooltipOverview
    // | [ "checkradio"; "installation" ] -> CheckradioInstallation
    // | [ "checkradio" ] -> CheckradioOverview
    // | [ "switch"; "installation" ] -> SwitchInstallation
    // | [ "switch" ] -> SwitchOverview
    // | [ "popover"; "installation" ] -> PopoverInstallation
    // | [ "popover" ] -> PopoverOverview
    // | [ "pageloader"; "installation" ] -> PageLoaderInstallation
    // | [ "pageloader" ] -> PageLoaderOverview
    | _ -> defaultPage

let getHref = function
    | Chia -> Router.format("")
    | ChiaInstallation -> Router.format("installation")
    | ChiaFileWriter -> Router.format("filewriter")
    | ChiaAiUtils -> Router.format("aiutils")
    | ChiaRedisCache -> Router.format("rediscache")
    | ChiaEventHub -> Router.format("eventhub")
    | ChiaCreateXml -> Router.format("createxml")
    | ChiaCreateTable -> Router.format("createtable")
    | ChiaCreateBlob -> Router.format("createblob")
    | ChiaCreateJsonBlob -> Router.format("createjsonblob")
    | ChiaPostToQueue -> Router.format("posttoqueue")
    | ChiaGetTableEntry -> Router.format("gettableentry")
    | ChiaLogger -> Router.format("logger")
    | ChiaInfrastructure -> Router.format("infrastructure")
    | ChiaExcelUtils -> Router.format("excelutils")
    | ChiaTableStorage -> Router.format("tablestorage")
    | ChiaClient -> Router.format("client")
    | ChiaClientInstallation -> Router.format("clientinstallation")
    | ChiaClientPageFlexer -> Router.format("clientpageflexer")
    // | BulmaInstallation -> Router.format("installation")
    // | BulmaAPIDescription -> Router.format("api-description")
    // | QuickViewOverview -> Router.format("quickview")
    // | QuickViewInstallation -> Router.format("quickview","installation")
    // | CalendarOverview -> Router.format("calendar")
    // | CalendarInstallation -> Router.format("calendar","installation")
    // | TooltipOverview -> Router.format("tooltip")
    // | TooltipInstallation -> Router.format("tooltip","installation")
    // | CheckradioOverview -> Router.format("checkradio")
    // | CheckradioInstallation -> Router.format("checkradio","installation")
    // | SwitchOverview -> Router.format("switch")
    // | SwitchInstallation -> Router.format("switch","installation")
    // | PopoverOverview -> Router.format("popover")
    // | PopoverInstallation -> Router.format("popover","installation")
    // | PageLoaderOverview -> Router.format("pageloader")
    // | PageLoaderInstallation -> Router.format("pageloader","installation")
