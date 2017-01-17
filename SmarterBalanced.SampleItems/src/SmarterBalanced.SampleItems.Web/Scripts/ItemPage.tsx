interface AccessibilityResource {
    defaultCode: string;
    description: string;
    disabled: boolean;
    label: string;
    selectedCode: string;
    resourceType: string;
    selections: Dropdown.Selection[];
}

namespace ItemPage {

    function toiSAAP(accResourceVM: AccessibilityResource[]): string {
        let str = "";
        for (let res of accResourceVM) {
            if (res.selectedCode && !res.disabled) {
                str += res.selectedCode + ";";
            }
        }
        return str;
    }

    function resetResource(model: AccessibilityResource): AccessibilityResource {
        const newModel = Object.assign({}, model);
        newModel.selectedCode = model.defaultCode;
        return newModel;
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

    export function getResourceTypes(resources: AccessibilityResource[]): string[] {
        let resourceTypes: string[] = [];
        for (const res of resources) {
            if (resourceTypes.indexOf(res.resourceType) === -1) {
                resourceTypes.push(res.resourceType);
            }
        }
        return resourceTypes;
    }

    export interface ViewModel {
        itemViewerServiceUrl: string;
        accessibilityCookieName: string;
        accResourceVMs: AccessibilityResource[];
        aboutItemVM: AboutItem.Props;
    }

    export interface Props extends ViewModel {
        onSave: (selections: AccessibilityModal.ResourceSelections) => void;
        onReset: () => void;
    }

    interface State { }

    export class Page extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            let accResourceVMs = this.props.accResourceVMs.map(addDisabledPlaceholder).sort((a, b) => {
                let aLabel = a.label.toLowerCase();
                let bLabel = b.label.toLowerCase();
                return (aLabel < bLabel) ? -1 : (aLabel > bLabel) ? 1 : 0;
            });
            this.state = { selections: {} };
        }

        saveOptions = (resourceSelections: AccessibilityModal.ResourceSelections): void => {
            this.props.onSave(resourceSelections);
        }

        resetOptions = (event: React.FormEvent): void => {
            event.preventDefault();
            this.props.onReset();
        }

        cancelChanges = (event: React.FormEvent): void => {
            event.preventDefault();
            this.setState({ selections: {} });
        }

        render() {
            let isaap = toiSAAP(this.props.accResourceVMs);
            let ivsUrl: string = this.props.itemViewerServiceUrl.concat("?isaap=", isaap);
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
                        accessibilityString={isaap}
                        url={ivsUrl} /> {/* TODO: remove redundant prop */}
                    <AboutItem.AIComponent {...this.props.aboutItemVM} />
                    <AccessibilityModal.ItemAccessibilityModal
                        localAccessibility={this.props.accResourceVMs}
                        onSave={this.props.onSave}
                        onReset={this.props.onReset} />
                    <Share.ShareModal iSAAP={isaap}/>
                </div>
            );
        }
    }

    export class Controller {
        constructor(private itemProps: ViewModel, private rootDiv: HTMLDivElement) { }

        onSave = (selections: AccessibilityModal.ResourceSelections) => {

            const newVMs: AccessibilityResource[] = [];
            for (let vm of this.itemProps.accResourceVMs) {
                const newVM = Object.assign({}, vm);
                newVM.selectedCode = selections[newVM.label] || newVM.selectedCode;
                newVMs.push(newVM);
            }
            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceVMs = newVMs;

            let cookieValue = generateAccCookieValue(this.itemProps.accResourceVMs);
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", btoa(cookieValue), "; path=/");

            this.render();
        }

        onReset = () => {
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", "", "; path=/");

            const newAccResourceVms = this.itemProps.accResourceVMs.map(resetResource);
            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceVMs = newAccResourceVms;
            
            this.render();
        }

        render() {
            ReactDOM.render(
                <Page {...this.itemProps} onSave={this.onSave} onReset={this.onReset} />,
                this.rootDiv);
        }
    }
}

function initializeItemPage(itemProps: ItemPage.ViewModel) {
    const controller = new ItemPage.Controller(
        itemProps,
        document.getElementById("item-container") as HTMLDivElement);
    controller.render();
}
