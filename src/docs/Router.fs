module Router

open Feliz.Router

type Page =
    | Fumble
    | QueryTable
    | HandleNullValues
    | ProvidingDefaultValues
    | ParameterizedQuery
    | InsertData

let defaultPage = Fumble

let parseUrl =
    function
    | [ "" ] -> Fumble
    | [ "querytable" ] -> QueryTable
    | [ "handlenullvalues" ] -> HandleNullValues
    | [ "providingdefaultvalues" ] -> ProvidingDefaultValues
    | [ "parameterizedquery" ] -> ParameterizedQuery
    | [ "insertdata" ] -> InsertData
    | _ -> defaultPage

let getHref =
    function
    | Fumble -> Router.format ("")
    | QueryTable -> Router.format ("querytable")
    | HandleNullValues -> Router.format ("handlenullvalues")
    | ProvidingDefaultValues -> Router.format ("providingdefaultvalues")
    | ParameterizedQuery -> Router.format ("parameterizedquery")
    | InsertData -> Router.format ("insertdata")
