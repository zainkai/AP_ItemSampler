interface AccessibilityResource {
    defaultCode: string;
    description: string;
    disabled: boolean;
    label: string;
    selectedCode: string;
    resourceTypeLabel: string;
    selections: Dropdown.Selection[];
}

namespace ItemPage {

    export interface Props {
        itemViewerServiceUrl: string;
        accessibilityCookieName: string;
        accResourceVMs: AccessibilityResource[];
        aboutItemVM: AboutItem.Props;
    }

    function getAccessibilityString(accResourceVM: AccessibilityResource[]): string {
        let str = "";
        for (let res of accResourceVM) {
            if (res.selectedCode && !res.disabled) {
                str += res.selectedCode + ";";
            }
        }
        return str;
    }

    function trimAccResource(model: AccessibilityResource): { label: string, selectedCode: string } {
        return {
            label: model.label,
            selectedCode: model.selectedCode,
        };
    }

    function generateAccCookieValue(accessibilityPrefs: AccessibilityResource[]): string {
        let newPrefs = accessibilityPrefs.map(trimAccResource);
        return JSON.stringify(newPrefs);
    }

    function addDisabledPlaceholder(resource: AccessibilityResource): AccessibilityResource {
        if (resource.disabled) {
            let newSelection = Object.assign(resource, resource);
            let disabledOption: Dropdown.Selection = {
                label: "Disabled for item",
                code: "",
                disabled: true,
            };
            newSelection.selections.push(disabledOption);
            newSelection.selectedCode = "";
            return newSelection;
        }
        return resource;
    }

    export function getResourceTypes(resources: AccessibilityResource[]): string[] {
        let resourceTypes: string[] = [];
        for (const res of resources) {
            if (resourceTypes.indexOf(res.resourceTypeLabel) === -1) {
                resourceTypes.push(res.resourceTypeLabel);
            }
        }
        return resourceTypes;
    }

    interface State {
        ivsAccOptions: string;
        accResourceVMs: AccessibilityResource[];
    }

    export class Page extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            let accResourceVMs = this.props.accResourceVMs.map(addDisabledPlaceholder).sort((a, b) => {
                let aLabel = a.label.toLowerCase();
                let bLabel = b.label.toLowerCase();
                return (aLabel < bLabel) ? -1 : (aLabel > bLabel) ? 1 : 0;
            });
            this.state = {
                ivsAccOptions: getAccessibilityString(this.props.accResourceVMs),
                accResourceVMs: accResourceVMs,
            };
        }

        updateResource = (code: string, label: string) => {
            const newResources = this.state.accResourceVMs.map((resource) => {
                if (resource.label === label) {
                    const newResource = Object.assign({}, resource);
                    newResource.selectedCode = code;
                    return newResource;
                }
                return resource;
            });
            this.setState({
                ivsAccOptions: this.state.ivsAccOptions,
                accResourceVMs: newResources,
            });
        }

        resetModelToDefault(model: AccessibilityResource): AccessibilityResource {
            const newModel = Object.assign({}, model);
            newModel.selectedCode = model.defaultCode;
            return newModel;
        }

        saveOptions = (event: React.FormEvent): void => {
            //Update Cookie with current options
            event.preventDefault();
            //copy the old accessibility resource view models
            let cookieValue = generateAccCookieValue(this.state.accResourceVMs);
            this.setState({
                accResourceVMs: this.state.accResourceVMs,
                ivsAccOptions: getAccessibilityString(this.state.accResourceVMs),
            });
            document.cookie = this.props.accessibilityCookieName.concat("=", btoa(cookieValue), "; path=/");
        }

        resetOptions = (event: React.FormEvent): void => {
            event.preventDefault();
            document.cookie = this.props.accessibilityCookieName.concat("=", "", "; path=/");
            let newAccResourceVms = this.state.accResourceVMs.map(this.resetModelToDefault);
            this.setState({
                ivsAccOptions: getAccessibilityString(newAccResourceVms),
                accResourceVMs: newAccResourceVms,
            });
        }

        resetAccForm = (event: React.FormEvent): void => {
            event.preventDefault();
            //TODO: reset the accessibility form to the currently set code
        }

        openAboutItemModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13) {
                const modal: any = ($("#about-item-modal-container"));
                modal.modal();
            }
        }

        // TODO: Update id with modal id
        openMoreLikeThisModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13) {
                const modal: any = ($("#TODO-modal-container"));
                modal.modal();
            }
        }

        openShareModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13) {
                const modal: any = ($("#share-modal-container"));
                modal.modal();
            }
        }

        openAccessibilityModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13) {
                const modal: any = ($("#accessibility-modal-container"));
                modal.modal();
            }
        }

        render() {
            let ivsUrl: string = this.props.itemViewerServiceUrl.concat("?isaap=", this.state.ivsAccOptions);
            const accText = (window.innerWidth < 800) ? "" : "Accessibility";
            return (
                <div>
                    <div className="btn-toolbar item-nav-group" role="toolbar" aria-label="Toolbar with button groups">
                        <div className="btn-group mr-2 item-nav-bottom" role="group" aria-label="First group">
                            <a className="btn item-nav-btn" data-toggle="modal" data-target="#about-item-modal-container"
                                onKeyUp={e => this.openAboutItemModal(e)} tabIndex={0}>
                                <span className="glyphicon glyphicon-info-sign glyphicon-pad" aria-hidden="true"></span>
                                About This Item
                            </a>

                            <a className="btn item-nav-btn" data-target="#share-modal-container"
                                onKeyUp={e => this.openMoreLikeThisModal(e)} tabIndex={0}>
                                <span className="glyphicon glyphicon-th-large glyphicon-pad" aria-hidden="true"></span>
                                More Like This
                            </a>
                            <a className="btn item-nav-btn" data-toggle="modal" data-target="#share-modal-container"
                                onKeyUp={e => this.openShareModal(e)} tabIndex={0}>
                                <span className="glyphicon glyphicon-share-alt glyphicon-pad" aria-hidden="true"></span>
                                Share
                            </a>
                        </div>

                        <div className="btn-group mr-2 pull-right" role="group" aria-label="Second group">
                            <a type="button" className="accessibility-btn btn btn-primary" data-toggle="modal"
                                data-target="#accessibility-modal-container"
                                onKeyUp={e => this.openAccessibilityModal(e)} tabIndex={0}>
                                <span className="glyphicon glyphicon-collapse-down" aria-hidden="true"></span>
                                <span className="accessibility-button-text"></span>
                            </a>
                        </div>
                    </div>
                    <ItemFrame baseUrl={this.props.itemViewerServiceUrl}
                        accessibilityString={this.state.ivsAccOptions}
                        url={ivsUrl}/>
                    <AboutItem.AIComponent {...this.props.aboutItemVM} />
                    <AccessibilityModal.ItemAccessibilityModal localAccessibility={this.state.accResourceVMs} updateSelection={this.updateResource} onSave={this.saveOptions} onReset={this.resetOptions} onCancel={this.resetAccForm} />
                    <Share.ShareModal iSAAP={getAccessibilityString(this.state.accResourceVMs)} />
                </div>
            );
        }
    }


}

function initializeItemPage(viewModel: ItemPage.Props) {
    ReactDOM.render(<ItemPage.Page {...viewModel} />,
        document.getElementById("item-container") as HTMLElement);
}
