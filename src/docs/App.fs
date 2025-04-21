module Docs.App

open Elmish
open Elmish.React
open Fable.Core.JsInterop

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

importSideEffects "./index.css"

Program.mkProgram View.init View.update View.AppView
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "safer-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
