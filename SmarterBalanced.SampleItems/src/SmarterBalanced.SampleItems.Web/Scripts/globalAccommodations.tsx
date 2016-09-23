// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

/// <reference path="Components/GlobalAccommodationsComponent.tsx" />

const example: Accommodation = {
    name: "Example",
    selectedIndex: 42,
    options: ["foo", "bar", "baz"]
}

let initialState: GlobalAccommodations.State = {
    presentation: example,
    textToSpeech: { selectedIndex: 0, options: [], name: "" },
    masking: { selectedIndex: 0, options: [], name: "" },
    colorContrast: { selectedIndex: 0, options: [], name: "" },
    highlighter: false,
    studentComments: { selectedIndex: 0, options: [], name: "" }
}

ReactDOM.render(
    <GlobalAccommodations.Component initialState={initialState} />,
    document.getElementById("container") as HTMLElement
);
