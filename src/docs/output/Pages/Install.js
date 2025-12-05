import { createElement } from "react";
import React from "react";
import * as react from "react";
import { reactApi } from "../fable_modules/Feliz.2.9.0/Interop.fs.js";
import { createObj } from "../fable_modules/fable-library-js.4.24.0/Util.js";
import { singleton, ofArray } from "../fable_modules/fable-library-js.4.24.0/List.js";

export function InstallView() {
    let elm_2, children, elems_2, elm_6, children_3, elems_6;
    const xs_8 = [createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(["Using NuGet package command"]),
    }), (elm_2 = singleton((children = singleton(createElement("pre", createObj(ofArray([["data-prefix", "$"], (elems_2 = [createElement("code", {
        children: ["Install-Package Fumble"],
    })], ["children", reactApi.Children.toArray(Array.from(elems_2))])])))), createElement("div", {
        className: "mockup-code",
        children: reactApi.Children.toArray(Array.from(children)),
    }))), createElement("div", {
        className: "max-w-xl",
        children: reactApi.Children.toArray(Array.from(elm_2)),
    })), createElement("div", {
        className: "description",
        children: reactApi.Children.toArray(["or Paket"]),
    }), (elm_6 = singleton((children_3 = singleton(createElement("pre", createObj(ofArray([["data-prefix", "$"], (elems_6 = [createElement("code", {
        children: ["paket add Fumble"],
    })], ["children", reactApi.Children.toArray(Array.from(elems_6))])])))), createElement("div", {
        className: "mockup-code",
        children: reactApi.Children.toArray(Array.from(children_3)),
    }))), createElement("div", {
        className: "max-w-xl",
        children: reactApi.Children.toArray(Array.from(elm_6)),
    }))];
    return react.createElement(react.Fragment, {}, ...xs_8);
}

