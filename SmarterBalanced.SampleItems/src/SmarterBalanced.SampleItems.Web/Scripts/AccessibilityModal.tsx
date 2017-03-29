namespace AccessibilityModal {
    interface Props {
        accResourceGroups: ItemPage.AccResourceGroup[];
        onSave(selections: ResourceSelections): void;
        onReset(): void;
    }

    export interface ResourceSelections {
        [resourceName: string]: string;
    }

    interface IsResourceExpanded {
        [resourceType: string]: boolean;
    }

    interface State {
        resourceTypeExpanded: IsResourceExpanded;
        resourceSelections: ResourceSelections;
    }

    export class ItemAccessibilityModal extends React.Component<Props, State> {

        constructor(props: Props) {
            super(props);

            const expandeds: IsResourceExpanded = {};
            const resourceTypes = this.props.accResourceGroups.map((res) => {
                return res.label;
            });
            for (const key of resourceTypes) {
                expandeds[key] = false;
            }
            this.state = {
                resourceTypeExpanded: expandeds,
                resourceSelections: {}
            };
        }

        toggleResourceType(resourceType: string) {
            const expandeds = Object.assign({}, this.state.resourceTypeExpanded || {});
            expandeds[resourceType] = !expandeds[resourceType];

            this.setState({
                resourceTypeExpanded: expandeds
            });
        }

        updateSelection = (code: string, label: string) => {
            const newSelections = Object.assign({}, this.state.resourceSelections || {});
            newSelections[label] = code;

            this.setState({ resourceSelections: newSelections });
        }

        onSave = (e: React.FormEvent<HTMLFormElement | HTMLButtonElement>) => {
            e.preventDefault();
            this.props.onSave(this.state.resourceSelections || {});
        }

        onCancel = (e: React.MouseEvent<HTMLButtonElement>) => {
            e.preventDefault();
            this.setState({ resourceSelections: {} });
        }

        onReset = (e: React.MouseEvent<HTMLButtonElement>) => {
            this.setState({ resourceSelections: {} });
            this.props.onReset();
        }

        renderResourceType(type: string) {
            let resources = this.props.accResourceGroups.filter(group => group.label === type)[0].accessibilityResources;
            let resourceTypeHeader = <h3>{type}</h3>;

            /* TODO: Remove after these accessibility resources are fixed */
            if (type === "Designated Support" || type === "Accommodations") {
                resourceTypeHeader = (
                    <div style={{ display: "flex", alignItems: "center"}}>
                        <h3 style={{ display: "inline-block" }}>{type}</h3>
                        <span style={{ marginTop: "10px" }}>&nbsp;&nbsp;(coming soon)</span>
                    </div>
                );
                for (let res of resources) {
                    res.selections = [];
                    res.disabled = true;
                    res.currentSelectionCode = "";
                    res.defaultSelection = "";
                }
            }
            /* TODO: REMOVE ABOVE */


            const resCount = resources.length;
            const isExpanded = (this.state.resourceTypeExpanded || {})[type];
            if (!isExpanded) {
                resources = resources.slice(0, 4);
            }

            let dropdowns = resources.map(res => {
                let selectedCode = (this.state.resourceSelections || {})[res.label] || res.currentSelectionCode;
                let ddprops: Dropdown.Props = {
                    defaultSelection: res.currentSelectionCode,
                    label: res.label,
                    selections: res.selections,
                    selectionCode: selectedCode,
                    disabled: res.disabled,
                    updateSelection: this.updateSelection,
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
                <div key={type}>
                    {resourceTypeHeader}
                    <div className="accessibility-dropdowns">
                        {dropdowns}
                    </div>
                    {expandButton}
                </div>
            );
        }

        render() {
            const types = ItemPage.getResourceTypes(this.props.accResourceGroups);
            const groups = types.map(t => this.renderResourceType(t));
            return (
                <div className="modal fade" id="accessibility-modal-container" tabIndex={-1} role="dialog" aria-labelledby="Accessibility Options Modal" aria-hidden="true">
                    <div className="modal-dialog accessibility-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close" onClick={this.onCancel}>
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title" id="myModalLabel">Accessibility Options</h4>
                            </div>
                            <div className="modal-body">
                                <p><span>Options displayed in grey are not available for this item.</span></p>
                                <p>
                                    To experience the <strong>text-to-speech functionality</strong>,&nbsp;
                                    please visit the&nbsp;
                                    <a href="http://www.smarterbalanced.org/assessments/practice-and-training-tests/ " target="_blank">Smarter Balanced Practice Test.</a>
                                   
                                </p>
                                <form id="accessibility-form" onSubmit={this.onSave}>
                                    <div className="accessibility-groups">
                                        {groups}
                                    </div>
                                </form>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary" form="accessibility-form" data-dismiss="modal" onClick={this.onSave}> Update</button>
                                <button className="btn btn-primary" data-dismiss="modal" onClick={this.onReset} >Reset to Default</button>
                                <button className="btn btn-primary btn-cancel" data-dismiss="modal" onClick={this.onCancel}>Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        } 
    }
}
