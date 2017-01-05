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
    }

    function getAccessibilityString(accResourceVM: AccessibilityResource[]): string {
        let str: string = "";
        for (let res of accResourceVM) {
            if (res.selectedCode) {
                str = str.concat(res.selectedCode, ";");
            }
        }
        return str;
    }

    function trimAccResource(model: AccessibilityResource): {label: string, selectedCode: string} {
        return ({
            label: model.label,
            selectedCode: model.selectedCode,
        });
    }

    function generateAccCookieValue(accessibilityPrefs: AccessibilityResource[]): string {
        let newPrefs = accessibilityPrefs.map(trimAccResource);
        return JSON.stringify(newPrefs);
    }

    interface State {
        ivsAccOptions: string;
        accResourceVMs: AccessibilityResource[];
    }

    export class Page extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = {
                ivsAccOptions: getAccessibilityString(this.props.accResourceVMs),
                accResourceVMs: this.props.accResourceVMs,
            };
            this.saveOptions = this.saveOptions.bind(this);
            this.resetOptions = this.resetOptions.bind(this);
            this.updateResource = this.updateResource.bind(this);
        }

        updateResource(code: string, label: string) {
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

        saveOptions(event: any): void {
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

        resetOptions(event: any): void {
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
                    <ul className="nav navbar-nav">
                        <li className="nav-item">
                            <a className="btn" data-toggle="modal" data-target="#share-modal-container" >About This Item</a>
                        </li>
                        <li className="nav-item">
                            <a className="btn" data-target="#share-modal-container" >More Like This</a>
                        </li>
                        <li className="nav-item">
                            <a className="btn" data-toggle="modal" data-target="#share-modal-container" >Share</a>
                        </li>
                    </ul>
                    <ItemFrame baseUrl={this.props.itemViewerServiceUrl}
                        accessibilityString={this.state.ivsAccOptions}
                        url={ivsUrl}/>
                    <AccessibilityModal.ItemAccessibilityModal localAccessibility={this.state.accResourceVMs} updateSelection={this.updateResource} onSave={this.saveOptions} onReset={this.resetOptions} />
                    <Share.ShareModal iSAAP={getAccessibilityString(this.state.accResourceVMs)}/>
                    <div className="navbar-fixed-bottom">
                        <a type="button" className="accessibility-button btn btn-primary" data-toggle="modal" data-target="#accessibility-modal-container">
                            <span className="glyphicon glyphicon-collapse-up" aria-hidden="true"></span>
                            <span className="accessibility-button-text"></span>
                        </a>
                    </div>
                </div>
            );
        }
    }


}

function initializeItemPage(viewModel: ItemPage.Props) {
    ReactDOM.render(<ItemPage.Page {...viewModel} />,
        document.getElementById("item-container") as HTMLElement);
}
