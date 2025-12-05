import { ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";
import { createObj, equals } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Helpers_combineClasses } from "../fable_modules/Feliz.Tailwind.3.0.9/Tailwind.fs.js";
import { reactApi } from "../fable_modules/Feliz.2.9.0/Interop.fs.js";
import { codedView } from "../SharedView.js";
import { map, delay, toList } from "../fable_modules/fable-library-js.4.24.0/Seq.js";

export const justifyItemsStyles = ofArray([["className", "justify-items-start"], ["className", "justify-items-end"], ["className", "justify-items-center"], ["className", "justify-items-stretch"], ["className", "justify-items-baseline"]]);

export function str(prop) {
    if (equals(prop, ["className", "justify-items-start"])) {
        return "justifyItems.justifyItemsStart";
    }
    else if (equals(prop, ["className", "justify-items-end"])) {
        return "justifyItems.justifyItemsEnd";
    }
    else if (equals(prop, ["className", "justify-items-center"])) {
        return "justifyItems.justifyItemsCenter";
    }
    else if (equals(prop, ["className", "justify-items-stretch"])) {
        return "justifyItems.justifyItemsStretch";
    }
    else if (equals(prop, ["className", "justify-items-baseline"])) {
        return "justifyItems.justifyItemsBaseline";
    }
    else {
        throw new Error("Unknown JustifyItems property");
    }
}

export function renderJustifyItems(justifyItems) {
    let elems;
    const example = createElement("div", createObj(Helpers_combineClasses("", ofArray([justifyItems, (elems = [createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "1"], ["className", "w-6 h-6 bg-gray-300"]])))), createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "2"], ["className", "w-6 h-6 bg-gray-300"]])))), createElement("div", createObj(Helpers_combineClasses("", ofArray([["children", "3"], ["className", "w-6 h-6 bg-gray-300"]]))))], ["children", reactApi.Children.toArray(Array.from(elems))])]))));
    const code = `Tailwind.divCombine [
            ${str(justifyItems)}
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
    const value_13 = "Use of JustifyItems is rather straightforward. Just add the JustifyItems property to the element you want to align. Here are some examples:";
    title = value_13;
    return codedView(title, code, example);
}

export function JustifyItemsView() {
    const xs = toList(delay(() => map(renderJustifyItems, justifyItemsStyles)));
    return react.createElement(react.Fragment, {}, ...xs);
}

