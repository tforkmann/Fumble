import { Record, Union } from "./fable_modules/fable-library-js.4.24.0/Types.js";
import { Page, Router_goToUrl, PageModule_toUrlSegments, Cmd_navigatePage, PageModule_parseFromUrlSegments, Page_$reflection } from "./Router.js";
import { record_type, union_type, string_type } from "./fable_modules/fable-library-js.4.24.0/Reflection.js";
import { RouterModule_router, RouterModule_trySeparateLast, RouterModule_encodeQueryString, RouterModule_encodeParts, RouterModule_urlSegments } from "./fable_modules/Feliz.Router.4.0.0/Router.fs.js";
import { Cmd_none } from "./fable_modules/Fable.Elmish.4.3.0/cmd.fs.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { equals, createObj } from "./fable_modules/fable-library-js.4.24.0/Util.js";
import { Helpers_combineClasses } from "./fable_modules/Feliz.DaisyUI.5.0.0/DaisyUI.fs.js";
import { append as append_1, ofArray, singleton } from "./fable_modules/fable-library-js.4.24.0/List.js";
import { reactApi } from "./fable_modules/Feliz.2.9.0/Interop.fs.js";
import { op_PlusPlus } from "./fable_modules/Feliz.DaisyUI.5.0.0/Operators.fs.js";
import { empty, singleton as singleton_1, append, delay, toList } from "./fable_modules/fable-library-js.4.24.0/Seq.js";
import { map, defaultArgWith } from "./fable_modules/fable-library-js.4.24.0/Option.js";
import { UseView } from "./Pages/Use.js";
import { QueryTableView } from "./Pages/QueryTable.js";
import { InsertDataView } from "./Pages/InsertData.js";
import { InstallView } from "./Pages/Install.js";

export class Msg extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["UrlChanged", "SetTheme"];
    }
}

export function Msg_$reflection() {
    return union_type("Docs.View.Msg", [], Msg, () => [[["Item", Page_$reflection()]], [["Item", string_type]]]);
}

export class State extends Record {
    constructor(Page, Theme) {
        super();
        this.Page = Page;
        this.Theme = Theme;
    }
}

export function State_$reflection() {
    return record_type("Docs.View.State", [], State, () => [["Page", Page_$reflection()], ["Theme", string_type]]);
}

export function init() {
    const nextPage = PageModule_parseFromUrlSegments(RouterModule_urlSegments(window.location.hash, 1));
    return [new State(nextPage, "corporate"), Cmd_navigatePage(nextPage)];
}

export function update(msg, state) {
    if (msg.tag === 1) {
        const theme = msg.fields[0];
        return [new State(state.Page, theme), Cmd_none()];
    }
    else {
        const page = msg.fields[0];
        return [new State(page, state.Theme), Cmd_none()];
    }
}

function rightSide(state, dispatch, title, docLink, elm) {
    let children_3, children_1, elm_1, elems_2, elements, elm_3;
    const children_5 = ofArray([(children_3 = singleton((children_1 = singleton((elm_1 = singleton(createElement("label", createObj(Helpers_combineClasses("btn", ofArray([["className", "btn-square"], ["className", "btn-ghost"], ["htmlFor", "main-menu"], (elems_2 = [createElement("svg", createObj(ofArray([["viewBox", (((((0 + " ") + 0) + " ") + 24) + " ") + 24], ["className", "inline-block w-6 h-6 stroke-current"], (elements = singleton(createElement("path", {
        d: "M4 6h16M4 12h16M4 18h16",
        strokeWidth: 2,
    })), ["children", reactApi.Children.toArray(Array.from(elements))])])))], ["children", reactApi.Children.toArray(Array.from(elems_2))])]))))), createElement("div", {
        className: "lg:hidden",
        children: reactApi.Children.toArray(Array.from(elm_1)),
    }))), createElement("div", {
        className: "navbar-start",
        children: reactApi.Children.toArray(Array.from(children_1)),
    }))), createElement("div", {
        className: "navbar",
        children: reactApi.Children.toArray(Array.from(children_3)),
    })), (elm_3 = ofArray([createElement("h2", createObj(ofArray([op_PlusPlus(["className", "text-primary"], ["className", "my-6 text-5xl font-bold"]), ["children", title]]))), elm]), createElement("div", {
        className: "px-5 py-5",
        children: reactApi.Children.toArray(Array.from(elm_3)),
    }))]);
    return createElement("div", {
        className: "drawer-content",
        children: reactApi.Children.toArray(Array.from(children_5)),
    });
}

function leftSide(p) {
    let elems_4, elm, elems_1, elems_3, children_3;
    const mi = (t, mp) => {
        const children = singleton(createElement("a", createObj(toList(delay(() => append(equals(p, mp) ? singleton_1(["className", "menu-active"]) : empty(), delay(() => append(singleton_1(["children", t]), delay(() => {
            let tupledArg, queryString;
            return append(singleton_1(["href", (tupledArg = PageModule_toUrlSegments(mp), (queryString = tupledArg[1], defaultArgWith(map((tupledArg_1) => {
                const r = tupledArg_1[0];
                const l = tupledArg_1[1];
                return RouterModule_encodeParts(append_1(r, singleton(l + RouterModule_encodeQueryString(queryString))), 1);
            }, RouterModule_trySeparateLast(tupledArg[0])), () => RouterModule_encodeParts(singleton(RouterModule_encodeQueryString(queryString)), 1))))]), delay(() => singleton_1(["onClick", (e) => {
                Router_goToUrl(e);
            }])));
        })))))))));
        return createElement("li", {
            children: reactApi.Children.toArray(Array.from(children)),
        });
    };
    const children_6 = ofArray([createElement("label", createObj(Helpers_combineClasses("drawer-overlay", singleton(["htmlFor", "main-menu"])))), createElement("aside", createObj(ofArray([["className", "flex flex-col border-r w-80 bg-base-100 text-base-content"], (elems_4 = [(elm = ofArray([createElement("span", {
        className: "text-primary",
        children: "Fumble",
    }), createElement("a", createObj(ofArray([["href", "https://www.nuget.org/packages/Fumble"], (elems_1 = [createElement("img", {
        src: "https://img.shields.io/nuget/v/Fumble.svg?style=flat-square",
    })], ["children", reactApi.Children.toArray(Array.from(elems_1))])])))]), createElement("div", {
        className: "inline-block text-3xl font-title px-5 py-5 font-bold",
        children: reactApi.Children.toArray(Array.from(elm)),
    })), createElement("ul", createObj(Helpers_combineClasses("menu", ofArray([["className", "menu-md"], ["className", "flex flex-col p-4 pt-0"], (elems_3 = [(children_3 = singleton(createElement("span", {
        children: ["Docs"],
    })), createElement("li", {
        className: "menu-title",
        children: reactApi.Children.toArray(Array.from(children_3)),
    })), mi("Install", new Page(0, [])), mi("Use", new Page(1, [])), mi("QueryTable", new Page(2, [])), mi("Insert Data", new Page(3, []))], ["children", reactApi.Children.toArray(Array.from(elems_3))])]))))], ["children", reactApi.Children.toArray(Array.from(elems_4))])])))]);
    return createElement("div", {
        className: "drawer-side",
        children: reactApi.Children.toArray(Array.from(children_6)),
    });
}

function inLayout(state, dispatch, title, docLink, p, elm) {
    let elems_1, elems;
    return createElement("div", createObj(ofArray([["className", "bg-base-100 text-base-content h-screen"], ["data-theme", state.Theme], (elems_1 = [createElement("div", createObj(Helpers_combineClasses("drawer", ofArray([["className", "lg:drawer-open"], (elems = [createElement("input", createObj(Helpers_combineClasses("drawer-toggle", ofArray([["type", "checkbox"], ["id", "main-menu"]])))), rightSide(state, dispatch, title, docLink, elm), leftSide(p)], ["children", reactApi.Children.toArray(Array.from(elems))])]))))], ["children", reactApi.Children.toArray(Array.from(elems_1))])])));
}

export function AppView(appViewInputProps) {
    let elements;
    const dispatch = appViewInputProps.dispatch;
    const state = appViewInputProps.state;
    let patternInput;
    const matchValue = state.Page;
    patternInput = ((matchValue.tag === 1) ? ["How to use", "/docs/use", createElement(UseView, null)] : ((matchValue.tag === 2) ? ["QueryTable", "/docs/use", createElement(QueryTableView, null)] : ((matchValue.tag === 3) ? ["Insert Data", "/docs/use", createElement(InsertDataView, null)] : ["Installation", "/docs/install", createElement(InstallView, null)])));
    const title = patternInput[0];
    const docLink = patternInput[1];
    const content = patternInput[2];
    return RouterModule_router(createObj(ofArray([["hashMode", 1], ["onUrlChanged", (arg_1) => {
        dispatch(new Msg(0, [PageModule_parseFromUrlSegments(arg_1)]));
    }], (elements = singleton(inLayout(state, dispatch, title, docLink, state.Page, content)), ["application", react.createElement(react.Fragment, {}, ...elements)])])));
}

