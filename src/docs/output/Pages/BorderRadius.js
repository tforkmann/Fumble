import { ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Helpers_combineClasses } from "../fable_modules/Feliz.DaisyUI.5.0.0/DaisyUI.fs.js";
import { codedView } from "../SharedView.js";
import { map, delay, toList } from "../fable_modules/fable-library-js.4.24.0/Seq.js";

export const borderRadiusStyles = ofArray([["className", "rounded-none"], ["className", "rounded-sm"], ["className", "rounded-md"], ["className", "rounded-lg"], ["className", "rounded-xl"], ["className", "rounded-2xl"], ["className", "rounded-3xl"]]);

export function str(prop) {
    if (equals(prop, ["className", "rounded-none"])) {
        return "rounded-none";
    }
    else if (equals(prop, ["className", "rounded-sm"])) {
        return "rounded-sm";
    }
    else if (equals(prop, ["className", "rounded-md"])) {
        return "rounded-md";
    }
    else if (equals(prop, ["className", "rounded-lg"])) {
        return "rounded-lg";
    }
    else if (equals(prop, ["className", "rounded-xl"])) {
        return "rounded-xl";
    }
    else if (equals(prop, ["className", "rounded-2xl"])) {
        return "rounded-2xl";
    }
    else if (equals(prop, ["className", "rounded-3xl"])) {
        return "rounded-3xl";
    }
    else {
        return "unknown";
    }
}

export function renderBorderRadius(borderRadius) {
    const example = createElement("button", createObj(Helpers_combineClasses("btn", ofArray([["className", "btn-outline"], ["className", "btn-primary"], ["className", "btn-lg"], borderRadius, ["children", str(borderRadius)]]))));
    const code = `Daisy.button.button
            [   button.outline
                button.primary
                button.lg
                ${str(borderRadius)}
                prop.text "This is ${str(borderRadius)}" ]`;
    const title = "Use of borderRadius is rather straightforward.";
    return codedView(title, code, example);
}

export function BorderRadiusView() {
    const xs = toList(delay(() => map(renderBorderRadius, borderRadiusStyles)));
    return react.createElement(react.Fragment, {}, ...xs);
}

