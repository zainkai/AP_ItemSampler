namespace AccessibilityModal {
    interface Props {
        localAccessibility: AccessibilityResource[];
        updateSelection(category: string, code: string): void;
        onSave(event: React.FormEvent): void;
        onReset(event: React.FormEvent): void;
        onCancel(event: React.FormEvent): void;
    }

    interface IsResourceExpanded {
        [resourceType: string]: boolean;
    }

    interface State {
        resourceTypeExpanded: IsResourceExpanded;
    }

    export class ItemAccessibilityModal extends React.Component<Props, State> {

        constructor(props: Props) {
            super(props);

            const expandeds: IsResourceExpanded = {};
            const resourceTypes = ItemPage.getResourceTypes(this.props.localAccessibility);
            for (const key of resourceTypes) {
                expandeds[key] = false;
            }
            this.state = {
                resourceTypeExpanded: expandeds
            };
        }

        toggleResourceType(resourceType: string) {
            const expandeds = Object.assign({}, this.state.resourceTypeExpanded);
            expandeds[resourceType] = !expandeds[resourceType];

            this.setState({
                resourceTypeExpanded: expandeds
            });
        }

        renderResourceType(type: string) {
            let matchingResources = this.props.localAccessibility.filter(res => res.resourceType === type);
            matchingResources.sort((a, b) => {
                if (!a.disabled && b.disabled) {
                    return -1;
                } else if (a.disabled && !b.disabled) {
                    return 1;
                } else {
                    return 0;
                }
            });

            const resCount = matchingResources.length;
            const isExpanded = this.state.resourceTypeExpanded[type];
            if (!isExpanded) {
                matchingResources = matchingResources.slice(0, 4);
            }

            let dropdowns = matchingResources.map(res => {
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

            let expandButton: JSX.Element | undefined;
            if (resCount <= 4) {
                expandButton = undefined;
            } else if (isExpanded) {
                expandButton =
                    <a className="expand-button"
                        onClick={() => this.toggleResourceType(type)}>

                        Show less
                    </a>;
            } else {
                expandButton =
                    <a className="expand-button"
                        onClick={() => this.toggleResourceType(type)}>

                        Show all
                    </a>;
            }

            return (
                <div>
                    <h3>{type || "Other"}</h3>
                    <div className="accessibility-dropdowns">
                        {dropdowns}
                    </div>
                    {expandButton}
                </div>
            );
        }

        render() {
            const types = ItemPage.getResourceTypes(this.props.localAccessibility);
            const groups = types.map(t => this.renderResourceType(t));
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
                                <p><span>Options highlighted in grey are not available for this item.</span></p>
                                <form id="accessibility-form" onSubmit={this.props.onSave}>
                                    <div className="accessibility-groups">
                                        {groups}
                                    </div>
                                </form>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary" form="accessibility-form" data-dismiss="modal" onClick={this.props.onSave}> Update</button>
                                <button className="btn btn-primary" data-dismiss="modal" onClick={this.props.onReset} >Reset to Default</button>
                                <button className="btn btn-primary btn-cancel" data-dismiss="modal" onClick={this.props.onCancel}>Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}
