// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

namespace GlobalAccommodations {
    export interface State {
        presentation: ComboBox.Props;
        textToSpeech: ComboBox.Props;
        masking: ComboBox.Props;
        colorContrast: ComboBox.Props;
        highlighter: boolean;
        studentComments: ComboBox.Props;
    }

    export interface Props {
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
            const { presentation, textToSpeech, masking, colorContrast, highlighter, studentComments } = this.state;
            return (
                <div>
                    <ComboBox.Component {...presentation} />
                    <ComboBox.Component {...textToSpeech} />
                    <ComboBox.Component {...masking} />
                    <ComboBox.Component {...colorContrast} />

                    <div>
                        <label htmlFor="select-highlighter">Highlighter:</label>
                        <input type="checkbox" id="select-highlighter" selected={highlighter}></input>
                    </div>

                    <ComboBox.Component {...studentComments} />
                </div>
            );
        }

        report() {
            alert(JSON.stringify(this.state));
        }
    }
}

namespace ComboBox {
    export interface Props {
        name: string;
        id: string;
        initialValue?: string;
        options: string[];
    }

    export interface State {
        value: string;
    }

    export class Component extends React.Component<Props, State> {
        state = {
            value: this.props.initialValue || this.props.options[0]
        };

        render() {
            const options: JSX.Element[] = this.props.options.map(name => <option key={this.props.id + "-" + name}>{name}</option>);
            return (
                // should event handlers be passed down?
                // e => this.props.onChange((e.target as HTMLInputElement).value)
                <div>
                    <label htmlFor={this.props.id}>{this.props.name}</label>
                    <select id={this.props.id} onChange={e => this.onChange(e)}> 
                        {options}
                    </select>
                </div>
            );
        }

        onChange(e: React.FormEvent) {
            this.setState({
                value: (e.target as HTMLInputElement).value
            });
        }
    }
}