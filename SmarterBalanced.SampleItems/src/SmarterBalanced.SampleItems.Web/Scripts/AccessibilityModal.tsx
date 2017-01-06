namespace AccessibilityModal {
    interface Props {
        localAccessibility: AccessibilityResource[];
        updateSelection(category: string, code: string): void;
        onSave(event: any): void;
        onReset(event: any): void;
    }

    export class ItemAccessibilityModal extends React.Component<Props, {}> {

        constructor(props: Props) {
            super(props);
        }

        render() {
            let dropdowns = this.props.localAccessibility.map(res => {
                let ddprops: Dropdown.Props = {
                    defaultSelection: res.selectedCode,
                    label: res.label,
                    selections: res.selections,
                    selectedCode: res.selectedCode,
                    disabled: res.disabled,
                    updateSelection: this.props.updateSelection,
                }
                return <Dropdown.Dropdown{...ddprops} key={res.label} />;
            });

            return (
                <div className="modal fade" id="accessibility-modal-container" tabIndex={-1} role="dialog" aria-labelledby="Accessibility Options Modal" aria-hidden="true">
                    <div className="modal-dialog accessibility-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title" id="myModalLabel">Accessibility Options</h4>
                            </div>
                            <div className="modal-body">
                                <p><span className="option-disabled">Options highlighted in grey are not avaiable for this item.</span></p>
                                <form id="accessibility-form" onSubmit={this.props.onSave}>
                                    <div className="accessibility-dropdowns">
                                        {dropdowns}
                                    </div>
                                </form>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary" form="accessibility-form" data-dismiss="modal" onClick={this.props.onSave}> Update</button>
                                <button className="btn btn-primary" data-dismiss="modal" onClick={this.props.onReset} >Reset to Default</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}
