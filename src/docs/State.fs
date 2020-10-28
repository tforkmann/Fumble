module State

open Domain
open Elmish
open Chia.Style
let init () = Model.init, Cmd.none

let private delay (msg:Msg) =
    async {
        do! Async.Sleep 2000
        return msg
    }

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | UrlChanged p -> { currentModel with CurrentPage = p }, Cmd.none
    | SentToast page -> currentModel, successToast (currentModel.CurrentPage.ToString()) page
    | ToggleLoader ->
        let cmd =
            if not currentModel.ShowLoader then Cmd.OfAsync.perform delay ToggleLoader id
            else Cmd.none
        { currentModel with ShowLoader = not currentModel.ShowLoader }, cmd
