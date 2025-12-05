module Docs.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

[<RequireQualifiedAccess>]
type Page =
    | Install
    | Use
    | QueryTable
    | NewFeatures
    // | HandleNullValues  //TODO: Implement this page
    // | ProvidingDefaultValues // TODO: Implement this page
    // | ParameterizedQuery // TODO: Implement this page
    | InsertData

module Page =
    let defaultPage = Page.Install

    let parseFromUrlSegments =
        function
        | [ "use" ] -> Page.Use
        | [ "querytable" ] -> Page.QueryTable
        | [ "newfeatures" ] -> Page.NewFeatures
        // | [ "handlenullvalues" ] -> Page.HandleNullValues
        // | [ "providingdefaultvalues" ] -> Page.ProvidingDefaultValues
        // | [ "parameterizedquery" ] -> Page.ParameterizedQuery
        | [ "insertdata" ] -> Page.InsertData
        | [] -> Page.Install
        | _ -> defaultPage

    let noQueryString segments : string list * (string * string) list = segments, []

    let toUrlSegments =
        function
        | Page.Install -> [] |> noQueryString
        | Page.Use -> [ "use" ] |> noQueryString
        | Page.QueryTable -> [ "querytable" ] |> noQueryString
        | Page.NewFeatures -> [ "newfeatures" ] |> noQueryString
        // | Page.HandleNullValues -> [ "handlenullvalues" ] |> noQueryString
        // | Page.ProvidingDefaultValues -> [ "providingdefaultvalues" ] |> noQueryString
        // | Page.ParameterizedQuery -> [ "parameterizedquery" ] |> noQueryString
        | Page.InsertData -> [ "insertdata" ] |> noQueryString

[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e: MouseEvent) =
        e.preventDefault ()
        let href: string = !!e.currentTarget?attributes?href?value
        Router.navigate href

    let navigatePage (p: Page) =
        p |> Page.toUrlSegments |> Router.navigate

[<RequireQualifiedAccess>]
module Cmd =
    let navigatePage (p: Page) = p |> Page.toUrlSegments |> Cmd.navigate
