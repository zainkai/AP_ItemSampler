interface AccessibilityResource {
    defaultCode: string;
    description: string;
    disabled: boolean;
    label: string;
    selectedCode: string;
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
                label: "Item is disabled",
                code: "",
                disabled: true,
            };
            newSelection.selections.push(disabledOption);
            newSelection.selectedCode = "";
            return newSelection;
        }
        return resource;
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

        render() {
            let ivsUrl: string = this.props.itemViewerServiceUrl.concat("?isaap=", this.state.ivsAccOptions);
            const accText = (window.innerWidth < 800) ? "" : "Accessibility";
            return (
                <div>
                    <ul className="nav navbar-nav mr-auto">
                        <li className="nav-item">
                            <a className="btn modal-toggle" data-toggle="modal" data-target="#about-item-modal-container" >
                                <span className="glyphicon glyphicon-th-list glyphicon-pad" aria-hidden="true"></span>
                                About This Item
                            </a>
                        </li>
                        <li className="nav-item">
                            <a className="btn modal-toggle" data-target="#share-modal-container" >
                                <span className="glyphicon glyphicon-th-large glyphicon-pad" aria-hidden="true"></span>
                                More Like This
                            </a>
                        </li>
                        <li className="nav-item">
                            <a className="btn modal-toggle" data-toggle="modal" data-target="#share-modal-container" >
                                <span className="glyphicon glyphicon-share-alt glyphicon-pad" aria-hidden="true"></span>
                                Share
                            </a>
                        </li>
                    </ul>
                    <a type="button" className="accessibility-button btn btn-primary" data-toggle="modal" data-target="#accessibility-modal-container">
                        <span className="glyphicon glyphicon-collapse-down" aria-hidden="true"></span>
                        <span className="accessibility-button-text"></span>
                    </a>
                    <ItemFrame baseUrl={this.props.itemViewerServiceUrl}
                        accessibilityString={this.state.ivsAccOptions}
                        url={ivsUrl} />
                    <AboutItem.AIComponent {...this.props.aboutItemVM} />
                    <AccessibilityModal.ItemAccessibilityModal localAccessibility={this.state.accResourceVMs} updateSelection={this.updateResource} onSave={this.saveOptions} onReset={this.resetOptions} />
                    <Share.ShareModal iSAAP={getAccessibilityString(this.state.accResourceVMs)}/>
                </div>
            );
        }
    }


}

function initializeItemPage(viewModel: ItemPage.Props) {
    ReactDOM.render(<ItemPage.Page {...viewModel} />,
        document.getElementById("item-container") as HTMLElement);
}
