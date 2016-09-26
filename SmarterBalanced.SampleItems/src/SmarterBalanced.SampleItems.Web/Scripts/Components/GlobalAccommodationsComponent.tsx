// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

namespace GlobalAccommodations {
    export interface State {
        selectedPresentation: string;
        selectedTextToSpeech: string;
        selectedMasking: string;
        selectedColorContrast: string;
        useHighlighter: boolean;
        selectedComments: string;
    }

    export interface Props {
        presentation: ComboBox.Data;
        textToSpeech: ComboBox.Data;
        masking: ComboBox.Data;
        colorContrast: ComboBox.Data;
        studentComments: ComboBox.Data;
        initialState: State
    }

    export class Component extends React.Component<Props, State> {
        state = this.props.initialState;

        render() {
            return (
                <div className="accommodations">
                    <h1>All Accommodations</h1>
                    {this.renderFields()}
                    <button onClick={() => this.report()}>Submit</button>
                </div>
            );
        }

        renderFields(): JSX.Element {
            const { presentation, textToSpeech, masking, colorContrast, studentComments } = this.props;
            return (
                <div>
                    <ComboBox.Component {...presentation} id={"presentation"} value={this.state.selectedPresentation} onChange={newValue => this.setState({ selectedPresentation: newValue } as State)} />
                    <ComboBox.Component {...textToSpeech} id={"text-to-speech"} value={this.state.selectedTextToSpeech} onChange={newValue => this.setState({ selectedTextToSpeech: newValue } as State)} />
                    <ComboBox.Component {...masking} id={"masking"} value={this.state.selectedMasking} onChange={newValue => this.setState({ selectedMasking: newValue } as State)} />
                    <ComboBox.Component {...colorContrast} id={"color-contrast"} value={this.state.selectedColorContrast} onChange={newValue => this.setState({ selectedColorContrast: newValue } as State)} />

                    <div>
                        <label htmlFor="select-highlighter">Highlighter:</label>
                        <input type="checkbox"
                            id="select-highlighter"
                            selected={this.state.useHighlighter}
                            onChange={e => this.setState({ useHighlighter: (e.target as HTMLInputElement).checked } as State)} ></input>
                    </div>

                    <ComboBox.Component {...studentComments} id={"student-comments"} value = { this.state.selectedComments } onChange= { newValue => this.setState({ selectedComments: newValue } as State) } />
                </div>
            );
        }

        report() {
            alert(JSON.stringify(this.state));
        }
    }
}

namespace ComboBox {
    export interface Data {
        title: string;
        options: string[];
        value?: string;
    }

    export interface Props extends Data {
        id: string;
        onChange: (value: string) => void;
    }

    export interface State { }

    export class Component extends React.Component<Props, State> {

        render() {
            const options: JSX.Element[] = this.props.options.map(name => <option key={this.props.id + "-" + name}>{name}</option>);
            return (
                <div>
                    <label htmlFor={this.props.id}>{this.props.title}</label>
                    <select id={this.props.id} value={this.props.value} onChange={e => this.onChange(e)}> 
                        {options}
                    </select>
                </div>
            );
        }

        onChange(e: React.FormEvent) {
            const newValue = (e.target as HTMLInputElement).value;
            this.props.onChange(newValue);
        }
    }
}