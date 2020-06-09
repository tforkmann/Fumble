module Domain

type Model = {
    CurrentPage : Router.Page
    ShowQuickView : bool
    ShowLoader : bool
}

module Model =
    let init = {
        CurrentPage = Router.defaultPage
        ShowQuickView = false
        ShowLoader = false
    }

type Msg =
    | UrlChanged of Router.Page
    | ToggleLoader
    | SentToast of string
