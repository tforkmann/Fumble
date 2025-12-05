import { singleton, ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Helpers_combineClasses } from "../fable_modules/Feliz.DaisyUI.5.0.0/DaisyUI.fs.js";
import { linedMockupCode, codedView } from "../SharedView.js";
import { reactApi } from "../fable_modules/Feliz.2.9.0/Interop.fs.js";

export const boxShadowStyles = ofArray([["className", "shadow-2xs"], ["className", "shadow-xs"], ["className", "shadow-sm"], ["className", "shadow-md"], ["className", "shadow-lg"], ["className", "shadow-xl"], ["className", "shadow-2xl"], ["className", "shadow-inner"], ["className", "shadow-none"]]);

export function str(prop) {
    if (equals(prop, ["className", "shadow-2xs"])) {
        return "boxShadow.shadow2Xs";
    }
    else if (equals(prop, ["className", "shadow-xs"])) {
        return "boxShadow.shadowXs";
    }
    else if (equals(prop, ["className", "shadow-sm"])) {
        return "boxShadow.shadowSm";
    }
    else if (equals(prop, ["className", "shadow-md"])) {
        return "boxShadow.shadowMd";
    }
    else if (equals(prop, ["className", "shadow-lg"])) {
        return "boxShadow.shadowLg";
    }
    else if (equals(prop, ["className", "shadow-xl"])) {
        return "boxShadow.shadowXl";
    }
    else if (equals(prop, ["className", "shadow-2xl"])) {
        return "boxShadow.shadow2Xl";
    }
    else if (equals(prop, ["className", "shadow-inner"])) {
        return "boxShadow.shadowInner";
    }
    else if (equals(prop, ["className", "shadow-none"])) {
        return "boxShadow.shadowNone";
    }
    else {
        throw new Error("Unknown BoxShadow property");
    }
}

export function renderBoxShadow(boxShadow) {
    const example = createElement("button", createObj(Helpers_combineClasses("btn", ofArray([["className", "btn-outline"], ["className", "btn-primary"], ["className", "btn-lg"], boxShadow, ["children", str(boxShadow)]]))));
    const code = `Daisy.button.button
                [   button.outline
                    button.primary
                    button.lg
                    ${str(boxShadow)}
                    prop.text "This is ${str(boxShadow)}" ]`;
    const title = "Use of borderRadius is rather straightforward.";
    return codedView(title, code, example);
}

export function QueryTableView() {
    let elm_2, elm_4;
    const xs_4 = [createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(["Fumble - QueryTable"]),
    }), (elm_2 = singleton(linedMockupCode("open Fumble")), createElement("div", {
        className: "max-w-xl",
        children: reactApi.Children.toArray(Array.from(elm_2)),
    })), (elm_4 = ofArray(["Now you can start using library. Everything important starts with ", createElement("code", {
        className: "code",
        children: "\n                    open Fumble\n                    let connectionString() = Env.getVar \"app_d\" ",
    })]), createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(Array.from(elm_4)),
    }))];
    return react.createElement(react.Fragment, {}, ...xs_4);
}

