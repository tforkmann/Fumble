import { ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Tailwind.3.0.9/Tailwind.fs.js";
import { reactApi } from "../fable_modules/Feliz.2.9.0/Interop.fs.js";
import { codedView } from "../SharedView.js";
import { map, delay, toList } from "../fable_modules/fable-library-js.4.24.0/Seq.js";

export const justifyContentStyles = ofArray([["className", "justify-start"], ["className", "justify-center"], ["className", "justify-end"], ["className", "justify-between"], ["className", "justify-around"], ["className", "justify-evenly"]]);

export function str(prop) {
    if (equals(prop, ["className", "justify-start"])) {
        return "justifyContent.justifyStart";
    }
    else if (equals(prop, ["className", "justify-center"])) {
        return "justifyContent.justifyCenter";
    }
    else if (equals(prop, ["className", "justify-end"])) {
        return "justifyContent.justifyEnd";
    }
    else if (equals(prop, ["className", "justify-between"])) {
        return "justifyContent.justifyBetween";
    }
    else if (equals(prop, ["className", "justify-around"])) {
        return "justifyContent.justifyAround";
    }
    else if (equals(prop, ["className", "justify-evenly"])) {
        return "justifyContent.justifyEvenly";
    }
    else {
        throw new Error("Unknown JustifyContent property");
    }
}

export function renderJustifyContent(justifyContent) {
    let elems;
    const example = createElement("div", createObj(Helpers_combineClasses("", ofArray([justifyContent, (elems = [createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "1"], ["className", "w-6 h-6 bg-gray-300"]])))), createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "2"], ["className", "w-6 h-6 bg-gray-300"]])))), createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "3"], ["className", "w-6 h-6 bg-gray-300"]]))))], ["children", reactApi.Children.toArray(Array.from(elems))])]))));
    const code = `Tailwind.divCombine [
            ${str(justifyContent)}
            prop.children [
                Tailwind.divCombine [
                    prop.text "1"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "2"
                    prop.className "w-6 h-6 bg-gray-300" ]
                Tailwind.divCombine [
                    prop.text "3"
                    prop.className "w-6 h-6 bg-gray-300" ]
            ]
        ]`;
    let title;
    const value_13 = "Use of JustifyContent is straightforward. Just add the JustifyContent property to the element you want to justify. Here are some examples:";
    title = value_13;
    return codedView(title, code, example);
}

export function JustifyContentView() {
    const xs = toList(delay(() => map(renderJustifyContent, justifyContentStyles)));
    return react.createElement(react.Fragment, {}, ...xs);
}

