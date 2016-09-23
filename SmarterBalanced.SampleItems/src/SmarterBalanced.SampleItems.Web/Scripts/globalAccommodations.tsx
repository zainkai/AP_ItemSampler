// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

/// <reference path="Components/GlobalAccommodationsComponent.tsx" />

const example: ComboBox.Props = {
    name: "Example",
    id: "select-example",
    options: ["foo", "bar", "baz"]
}

let initialState: GlobalAccommodations.State = {
    presentation: example,
    textToSpeech: { options: [], name: "", id: "s1" },
    masking: { options: [], name: "", id: "s2" },
    colorContrast: { options: [], name: "", id: "s3" },
    highlighter: false,
    studentComments: { options: [], name: "", id: "s4" }
}

function clone<T>(input: T): T {
    return JSON.parse(JSON.stringify(input));
}

namespace GlobalAccommodations {
    export class Controller {
        constructor(private state: GlobalAccommodations.State) { }

        render() {
            ReactDOM.render(
                <GlobalAccommodations.Component initialState={this.state} />,
                document.getElementById("container") as HTMLElement
            );
        }
    }

}

new GlobalAccommodations.Controller(initialState).render();