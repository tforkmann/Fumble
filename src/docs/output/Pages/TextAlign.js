import { ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { codedView } from "../SharedView.js";
import { map, delay, toList } from "../fable_modules/fable-library-js.4.24.0/Seq.js";

export const textAlignStyles = ofArray([["className", "text-left"], ["className", "text-center"], ["className", "text-right"], ["className", "text-justify"], ["className", "text-start"], ["className", "text-end"]]);

export function str(prop) {
    if (equals(prop, ["className", "text-left"])) {
        return "textAlign.left";
    }
    else if (equals(prop, ["className", "text-center"])) {
        return "textAlign.center";
    }
    else if (equals(prop, ["className", "text-right"])) {
        return "textAlign.right";
    }
    else if (equals(prop, ["className", "text-justify"])) {
        return "textAlign.justify";
    }
    else if (equals(prop, ["className", "text-start"])) {
        return "textAlign.start";
    }
    else if (equals(prop, ["className", "text-end"])) {
        return "textAlign.end";
    }
    else {
        throw new Error("Unknown TextAlign property");
    }
}

export function renderTextAlign(textAlign) {
    let value;
    const example = createElement("p", createObj(ofArray([textAlign, (value = "So I started to walk into the water. I won\'t lie to you boys, I was terrified. But I pressed on, and as I made my way past the breakers a strange calm came over me. I don\'t know if it was divine intervention or the kinship of all living things but I tell you Jerry at that moment, I was a marine biologist.", ["children", value])])));
    const code = `Html.p [
            ${str(textAlign)}
            prop.text "So I started to walk into the water. I won't lie to you`;
    let title;
    const value_2 = "Use of TextAlign is straightforward. Just add the TextAlign property to the element you want to align. Here are some examples:";
    title = value_2;
    return codedView(title, code, example);
}

export function TextAlignView() {
    const xs = toList(delay(() => map(renderTextAlign, textAlignStyles)));
    return react.createElement(react.Fragment, {}, ...xs);
}

