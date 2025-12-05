import { createElement } from "react";
import React from "react";
import * as react from "react";
import { reactApi } from "../fable_modules/Feliz.2.9.0/Interop.fs.js";
import { linedMockupCode } from "../SharedView.js";
import { ofArray, singleton } from "../fable_modules/fable-library-js.4.24.0/List.js";

export function UseView() {
    let elm_2, elm_4;
    const xs_4 = [createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(["After installation just open proper namespace:"]),
    }), (elm_2 = singleton(linedMockupCode("open Fumble")), createElement("div", {
        className: "max-w-xl",
        children: reactApi.Children.toArray(Array.from(elm_2)),
    })), (elm_4 = ofArray(["Now you can start using library. Everything important starts with ", createElement("code", {
        className: "code",
        children: "Fumble.*",
    }), " module."]), createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(Array.from(elm_4)),
    }))];
    return react.createElement(react.Fragment, {}, ...xs_4);
}

