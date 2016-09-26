// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

/// <reference path="Components/GlobalAccommodationsComponent.tsx" />

const presentationData = { title: "Presentation:", options: ["English", "Spanish", "Braille"] };
const textToSpeechData = { title: "Text-To-Speech:", options: ["No Text-To-Speech", "Passages and Items", "Stimuli", "Items"] };
const maskingData = { title: "Masking:", options: ["Masking Not Available", "Masking Available"] };
const colorContrastData = { title: "Color Contrast:", options: ["Black on White (Default)", "Black on Rose", "Yellow on Blue", "Medium Gray on Light Gray", "Reverse Contrast"] };
const commentsData = { title: "Student Comments:", options: ["Notepad", "Off"] };

const props: GlobalAccommodations.Props = {
    presentation: presentationData,
    textToSpeech: textToSpeechData,
    masking: maskingData,
    colorContrast: colorContrastData,
    studentComments: commentsData,
    initialState: {
        selectedPresentation: presentationData.options[0],
        selectedTextToSpeech: commentsData.options[0],
        selectedMasking: maskingData.options[0],
        selectedColorContrast: colorContrastData.options[0],
        selectedComments: commentsData.options[0],
        useHighlighter: false
    }
};

// This doesn't really do anything right now
namespace GlobalAccommodations {
    export class Controller {
        constructor(private props: GlobalAccommodations.Props) { }

        render() {
            ReactDOM.render(
                <GlobalAccommodations.Component {...this.props} />,
                document.getElementById("container") as HTMLElement
            );
        }
    }

}

new GlobalAccommodations.Controller(props).render();