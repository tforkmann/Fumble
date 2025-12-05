import { Union } from "./fable_modules/fable-library-js.4.24.0/Types.js";
import { union_type } from "./fable_modules/fable-library-js.4.24.0/Reflection.js";
import { append, singleton, empty, tail, head, isEmpty } from "./fable_modules/fable-library-js.4.24.0/List.js";
import { RouterModule_trySeparateLast, RouterModule_encodeQueryString, RouterModule_nav } from "./fable_modules/Feliz.Router.4.0.0/Router.fs.js";
import { map, defaultArgWith } from "./fable_modules/fable-library-js.4.24.0/Option.js";
import { Cmd_ofEffect } from "./fable_modules/Fable.Elmish.4.3.0/cmd.fs.js";

export class Page extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Install", "Use", "QueryTable", "InsertData"];
    }
}

export function Page_$reflection() {
    return union_type("Docs.Router.Page", [], Page, () => [[], [], [], []]);
}

export const PageModule_defaultPage = new Page(0, []);

export function PageModule_parseFromUrlSegments(_arg) {
    let matchResult;
    if (isEmpty(_arg)) {
        matchResult = 3;
    }
    else {
        switch (head(_arg)) {
            case "use": {
                if (isEmpty(tail(_arg))) {
                    matchResult = 0;
                }
                else {
                    matchResult = 4;
                }
                break;
            }
            case "querytable": {
                if (isEmpty(tail(_arg))) {
                    matchResult = 1;
                }
                else {
                    matchResult = 4;
                }
                break;
            }
            case "insertdata": {
                if (isEmpty(tail(_arg))) {
                    matchResult = 2;
                }
                else {
                    matchResult = 4;
                }
                break;
            }
            default:
                matchResult = 4;
        }
    }
    switch (matchResult) {
        case 0:
            return new Page(1, []);
        case 1:
            return new Page(2, []);
        case 2:
            return new Page(3, []);
        case 3:
            return new Page(0, []);
        default:
            return PageModule_defaultPage;
    }
}

export function PageModule_noQueryString(segments) {
    return [segments, empty()];
}

export function PageModule_toUrlSegments(_arg) {
    switch (_arg.tag) {
        case 1:
            return PageModule_noQueryString(singleton("use"));
        case 2:
            return PageModule_noQueryString(singleton("querytable"));
        case 3:
            return PageModule_noQueryString(singleton("insertdata"));
        default:
            return PageModule_noQueryString(empty());
    }
}

export function Router_goToUrl(e) {
    e.preventDefault();
    const href = e.currentTarget.attributes.href.value;
    RouterModule_nav(singleton(href), 1, 1);
}

export function Router_navigatePage(p) {
    const tupledArg = PageModule_toUrlSegments(p);
    const queryString = tupledArg[1];
    defaultArgWith(map((tupledArg_1) => {
        const r = tupledArg_1[0];
        const l = tupledArg_1[1];
        RouterModule_nav(append(r, singleton(l + RouterModule_encodeQueryString(queryString))), 1, 1);
    }, RouterModule_trySeparateLast(tupledArg[0])), () => {
        RouterModule_nav(singleton(RouterModule_encodeQueryString(queryString)), 1, 1);
    });
}

export function Cmd_navigatePage(p) {
    const tupledArg = PageModule_toUrlSegments(p);
    return Cmd_ofEffect((_arg_1) => {
        const queryString_1 = tupledArg[1];
        defaultArgWith(map((tupledArg_1) => {
            const r = tupledArg_1[0];
            const l = tupledArg_1[1];
            RouterModule_nav(append(r, singleton(l + RouterModule_encodeQueryString(queryString_1))), 1, 1);
        }, RouterModule_trySeparateLast(tupledArg[0])), () => {
            RouterModule_nav(singleton(RouterModule_encodeQueryString(queryString_1)), 1, 1);
        });
    });
}

