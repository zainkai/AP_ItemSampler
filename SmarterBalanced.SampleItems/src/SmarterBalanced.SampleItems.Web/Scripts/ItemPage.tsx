interface AccessibilityResource {
    code: string; // ID for this resource
    defaultSelection: string;
    description: string;
    disabled: boolean;
    label: string;
    selectedCode: string; // ID of the current selection
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
        return encodeURIComponent(str);
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

    function toCookie(accGroups: AccResourceGroup[]): string {
        let prefs: AccessibilityModal.ResourceSelections = {};
        for (const group of accGroups) {
            for (const resource of group.accessibilityResources) {
                prefs[resource.code] = resource.selectedCode;
            }
        }

        const json = JSON.stringify(prefs);
        const cookie = btoa(json);
        return cookie;
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
        aboutThisItemVM: AboutThisItem.Props;
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

        openAboutItemModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13 || e.keyCode === 23) {
                const modal: any = ($("#about-item-modal-container"));
                modal.modal();
            }
        }

        // TODO: Update id with modal id
        openMoreLikeThisModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13 || e.keyCode === 23) {
                const modal: any = ($("#TODO-modal-container"));
                modal.modal();
            }
        }

        openShareModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13 || e.keyCode === 23) {
                const modal: any = ($("#share-modal-container"));
                modal.modal();
            }
        }

        openAccessibilityModal(e: React.KeyboardEvent) {
            if (e.keyCode === 13 || e.keyCode === 23) {
                const modal: any = ($("#accessibility-modal-container"));
                modal.modal();
            }
        }

        render() {
            let isaap = toiSAAP(this.props.accResourceGroups);
            let ivsUrl: string = this.props.itemViewerServiceUrl.concat("?isaap=", isaap);
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
                    <ItemFrame url={ivsUrl} />
                    <AboutThisItem.ATIComponent {...this.props.aboutThisItemVM} />
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

            let cookieValue = toCookie(this.itemProps.accResourceGroups);
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", cookieValue, "; path=/");

            this.render();
        }

        onReset = () => {
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", "", "; path=/");
            
            const newAccResourceGroups = this.itemProps.accResourceGroups.map(g => {
                const newGroup = Object.assign({}, g);
                newGroup.accessibilityResources = newGroup.accessibilityResources.map(resetResource);
                return newGroup;
            });

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
