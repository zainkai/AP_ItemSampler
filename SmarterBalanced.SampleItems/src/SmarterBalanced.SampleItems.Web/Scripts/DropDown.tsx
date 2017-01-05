namespace Dropdown {
    export interface Selection {
        disabled: boolean;
        label: string;
        code: string;
    }

    export interface Props {
        defaultSelection: string;
        label: string;
        selectedCode: string;
        selections: Selection[];
        updateSelection(code: string, label: string): void;
    }

    export class Dropdown extends React.Component<Props, {}> {
        constructor(props: Props) {
            super(props);
            this.onChange = this.onChange.bind(this);
        }

        onChange(event: any): void {
            this.props.updateSelection(event.target.value, this.props.label);
        }

        renderOption(selection: Selection) {
            const disabledCSS: string = selection.disabled ? "option-disabled" : "option-enabled";
            return (
                <option value={selection.code}
                    disabled={selection.disabled}
                    key={selection.label}
                    className={disabledCSS}>

                    {selection.label}
                </option>
            );
        }

        render() {
            const options = this.props.selections.map(this.renderOption);
            return (
                <div className="accessibility-dropdown form-group">
                    <label>{this.props.label}</label><br />
                    <select className="form-control"
                        onChange={this.onChange}
                        value={this.props.selectedCode}>
                        {options}
                    </select>
                </div>
            );
        }
    }
}