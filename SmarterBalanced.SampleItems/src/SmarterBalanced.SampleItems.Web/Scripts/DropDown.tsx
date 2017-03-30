namespace Dropdown {
    export interface Selection {
        disabled: boolean;
        label: string;
        selectionCode: string;
        order: number;
        hidden: boolean;
    }

    export interface Props {
        label: string;
        disabled: boolean;
        selectionCode: string;
        selections: Selection[];
        updateSelection(code: string, label: string): void;
        defaultSelection: string;
    }

    export class Dropdown extends React.Component<Props, {}> {
        constructor(props: Props) {
            super(props);
        }

        onChange = (event: React.FormEvent<HTMLSelectElement>) => {
            this.props.updateSelection(event.currentTarget.value, this.props.label);
        }

        renderOption = (selection: Selection) => {
            const disabledCSS: string = selection.disabled ? "option-disabled" : "option-enabled";
            const label = this.props.disabled ? "" : selection.label;
            return (
                <option value={selection.selectionCode}
                    disabled={selection.disabled}
                    key={selection.selectionCode}
                    className={disabledCSS}
                    selected={this.props.selectionCode === selection.selectionCode}>
                    {label}
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
                        value={this.props.selectionCode}>
                        {options}
                    </select>
                </div>
            );
        }
    }
}