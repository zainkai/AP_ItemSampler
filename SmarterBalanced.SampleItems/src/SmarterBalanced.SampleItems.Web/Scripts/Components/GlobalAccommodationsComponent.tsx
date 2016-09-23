// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

interface Accommodation {
    name: string;
    selectedIndex: number;
    options: string[];
}

namespace GlobalAccommodations {
    export interface State {
        presentation: Accommodation;
        textToSpeech: Accommodation;
        masking: Accommodation;
        colorContrast: Accommodation;
        highlighter: boolean;
        studentComments: Accommodation;
    }

    export interface Props {
        initialState: State
    }

    export class Component extends React.Component<Props, State> {
        state = this.props.initialState;

        render() {
            const { presentation, textToSpeech, masking, colorContrast, highlighter, studentComments } = this.state;
            return (
                <div>
                    <div>
                        <p style={{ display: "inline-block" }}>Presentation:</p>
                        {comboBox(presentation)}
                    </div>

                    <div>
                        <p style={{ display: "inline-block" }}>Text-To-Speech:</p>
                        {comboBox(textToSpeech)}
                    </div>

                    <div>
                        <p style={{ display: "inline-block" }}>Masking:</p>
                        {comboBox(masking)}
                    </div>

                    <div>
                        <p style={{ display: "inline-block" }}>Color Contrast:</p>
                        {comboBox(colorContrast)}
                    </div>

                    <div>
                        <p style={{ display: "inline-block" }}>Highlighter:</p>
                        <input type="checkbox" selected={highlighter}></input>
                    </div>

                    <div>
                        <p style={{ display: "inline-block" }}>Student Comments:</p>
                        {comboBox(studentComments)}
                    </div>
                </div>
            );
        }
    }

    function comboBox(accommodation: Accommodation): JSX.Element {
        const options: JSX.Element[] = accommodation.options.map(function (name, idx) {
            const isSelected = idx === accommodation.selectedIndex;
            return <option selected={isSelected}>{name}</option>;
        });

        return (
            <select>
                {options}
            </select>
        );
    }
}
