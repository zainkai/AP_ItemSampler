﻿interface AccessibilityResource {
    defaultSelection: string;
    description: string;
    disabled: boolean;
    label: string;
    selectedCode: string;
    resourceTypeLabel: string;
    order: number;
    selections: Dropdown.Selection[];
}

namespace ItemPage {

    function toiSAAP(accResourceGroups: AccResourceGroup[]): string {
        let str = "";
        for (let group of accResourceGroups) {
            for (let res of group.accessibilityResources) {
                if (res.selectedCode && !res.disabled) {
                    str += res.selectedCode + ";";
                }
            }
        }
        return str;
    }

    function resetResource(model: AccessibilityResource): AccessibilityResource {
        const newModel = Object.assign({}, model);
        newModel.selectedCode = model.defaultSelection;
        return newModel;
    }

    function trimAccResource(resource: AccessibilityResource): { label: string, selectedCode: string } {
        return {
            label: resource.label,
            selectedCode: resource.selectedCode,
        };
    }

    function generateAccCookieValue(accGroups: AccResourceGroup[]): string {
        let cookieValue = "";
        let newGroups = [];
        for (let group of accGroups) {
            newGroups.push({
                label: group.label,
                order: group.order,
                accessibilityResources: group.accessibilityResources.map(trimAccResource)
            });
        }
        return JSON.stringify(newGroups);
    }

    function addDisabledPlaceholder(resource: AccessibilityResource): AccessibilityResource {
        if (resource.disabled) {
            let newSelection = Object.assign(resource, resource);
            let disabledOption: Dropdown.Selection = {
                label: "Disabled for item",
                code: "",
                disabled: true,
                order: 0,
            };
            newSelection.selections.push(disabledOption);
            newSelection.selectedCode = "";
            return newSelection;
        }
        return resource;
    }

    export function getResourceTypes(resourceGroups: AccResourceGroup[]): string[] {
        let resourceTypes: string[] = [];
        for (const group of resourceGroups) {
            if (resourceTypes.indexOf(group.label) === -1) {
                resourceTypes.push(group.label);
            }
        }
        return resourceTypes;
    }

    export interface AccResourceGroup {
        label: string;
        order: number;
        accessibilityResources: AccessibilityResource[];
    }

    export interface ViewModel {
        itemViewerServiceUrl: string;
        accessibilityCookieName: string;
        accResourceGroups: AccResourceGroup[];
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
            let isaap = toiSAAP(this.props.accResourceGroups);
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
                        accResourceGroups={this.props.accResourceGroups}
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

            const newGroups: AccResourceGroup[] = [];
            for (let group of this.itemProps.accResourceGroups) {
                const newGroup = Object.assign({}, group);
                const newResources: AccessibilityResource[] = [];
                for (let res of newGroup.accessibilityResources) {
                    const newRes = Object.assign({}, res);
                    newRes.selectedCode = selections[newRes.label] || newRes.selectedCode;
                    newResources.push(newRes);
                }
                newGroup.accessibilityResources = newResources;
                newGroups.push(newGroup);
            }
            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceGroups = newGroups;

            let cookieValue = generateAccCookieValue(this.itemProps.accResourceGroups);
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", btoa(cookieValue), "; path=/");

            this.render();
        }

        onReset = () => {
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", "", "; path=/");

            const newAccResourceGroups = Object.assign({}, this.itemProps.accResourceGroups);
            for (let group of newAccResourceGroups) {
                group.accessibilityResources = group.accessibilityResources.map(resetResource);
            }
            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceGroups = newAccResourceGroups;
            
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
