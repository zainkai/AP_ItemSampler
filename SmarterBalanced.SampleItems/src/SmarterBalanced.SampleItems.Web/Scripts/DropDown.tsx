namespace Dropdown {
    export interface Selection {
        disabled: boolean;
        label: string;
        code: string;
        order: number;
    }

    export interface Props {
        defaultSelection: string;
        label: string;
        disabled: boolean;
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
            const classes = "accessibility-dropdown form-group ".concat(this.props.disabled ? "selection-disabled" : "selection-enabled");
            const options = this.props.selections.map(this.renderOption);
            return (
                <div className={classes}>
                    <label>{this.props.label}</label><br />
                    <select className="form-control"
                        disabled={this.props.disabled}
                        onChange={this.onChange}
                        value={this.props.selectedCode}>
                        {options}
                    </select>
                </div>
            );
        }
    }
}