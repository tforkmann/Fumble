import { ProgramModule_mkProgram, ProgramModule_withConsoleTrace, ProgramModule_run } from "./fable_modules/Fable.Elmish.4.3.0/program.fs.js";
import { Program_withReactSynchronous } from "./fable_modules/Fable.Elmish.React.4.0.0/react.fs.js";
import { AppView, update, init } from "./View.js";
import { createElement } from "react";
import "../index.css";


ProgramModule_run(Program_withReactSynchronous("safer-app", ProgramModule_withConsoleTrace(ProgramModule_mkProgram(init, update, (state_1, dispatch) => createElement(AppView, {
    state: state_1,
    dispatch: dispatch,
})))));

