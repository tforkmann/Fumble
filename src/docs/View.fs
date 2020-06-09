module View

open Domain
open Feliz
open Feliz.Bulma
open Router

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
        [ Bulma.menuLabel "Fumble"
          Bulma.menuList [
              item "Overview" Fumble ] ]

let contentPart model dispatch =
    match model.CurrentPage with
    | Fumble -> Fumble.overview

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
