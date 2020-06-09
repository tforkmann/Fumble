module Router

open Feliz.Router

type Page =
    | Fumble


let defaultPage = Fumble

let parseUrl = function
    | [ "" ] -> Fumble
    | _ -> defaultPage

let getHref = function
    | Fumble -> Router.format("")
