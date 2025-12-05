import { ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Helpers_combineClasses } from "../fable_modules/Feliz.DaisyUI.5.0.0/DaisyUI.fs.js";
import { codedView } from "../SharedView.js";
import { map, delay, toList } from "../fable_modules/fable-library-js.4.24.0/Seq.js";

export const fontSizeStyles = ofArray([["className", "text-xs"], ["className", "text-sm"], ["className", "text-base"], ["className", "text-lg"], ["className", "text-xl"], ["className", "text-2xl"], ["className", "text-3xl"], ["className", "text-4xl"], ["className", "text-5xl"], ["className", "text-6xl"], ["className", "text-7xl"], ["className", "text-8xl"], ["className", "text-9xl"]]);

export function str(prop) {
    if (equals(prop, ["className", "text-xs"])) {
        return "text-xs";
    }
    else if (equals(prop, ["className", "text-sm"])) {
        return "text-sm";
    }
    else if (equals(prop, ["className", "text-base"])) {
        return "text-base";
    }
    else if (equals(prop, ["className", "text-lg"])) {
        return "text-lg";
    }
    else if (equals(prop, ["className", "text-xl"])) {
        return "text-xl";
    }
    else if (equals(prop, ["className", "text-2xl"])) {
        return "text-2xl";
    }
    else if (equals(prop, ["className", "text-3xl"])) {
        return "text-3xl";
    }
    else if (equals(prop, ["className", "text-4xl"])) {
        return "text-4xl";
    }
    else if (equals(prop, ["className", "text-5xl"])) {
        return "text-5xl";
    }
    else if (equals(prop, ["className", "text-6xl"])) {
        return "text-6xl";
    }
    else if (equals(prop, ["className", "text-7xl"])) {
        return "text-7xl";
    }
    else if (equals(prop, ["className", "text-8xl"])) {
        return "text-8xl";
    }
    else if (equals(prop, ["className", "text-9xl"])) {
        return "text-9xl";
    }
    else {
        return "unknown";
    }
}

export function renderFontSize(fontSize) {
    const example = createElement("button", createObj(Helpers_combineClasses("btn", ofArray([["className", "btn-outline"], ["className", "btn-primary"], ["className", "btn-lg"], fontSize, ["children", str(fontSize)]]))));
    const code = `Daisy.button.button
            [   button.outline
                button.primary
                button.lg
                ${str(fontSize)}
                prop.text "This is ${str(fontSize)}" ]`;
    const title = "Use of fontSize is rather straightforward.";
    return codedView(title, code, example);
}

export function FontSizeView() {
    const xs = toList(delay(() => map(renderFontSize, fontSizeStyles)));
    return react.createElement(react.Fragment, {}, ...xs);
}

