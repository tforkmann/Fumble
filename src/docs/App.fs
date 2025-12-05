module Docs.App

open Elmish
open Elmish.React
open Fable.Core.JsInterop

importSideEffects "./index.css"

Program.mkProgram View.init View.update View.AppView
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "safer-app"
|> Program.run
