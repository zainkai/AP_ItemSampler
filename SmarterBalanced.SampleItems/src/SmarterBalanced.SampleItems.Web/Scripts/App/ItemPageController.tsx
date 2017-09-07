import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as Accessibility from '../Accessibility/Accessibility';
import * as AboutThisItem from '../AboutItem/AboutThisItem';
import { ItemPage } from './ItemPage'

export namespace ItemPageController{

    export class Controller {
        constructor(private itemProps: ItemPage.ViewModel, private rootDiv: HTMLDivElement) {
            this.itemProps.currentItem = Accessibility.isBrailleEnabled(this.itemProps.accResourceGroups) ? this.itemProps.brailleItem : this.itemProps.nonBrailleItem;
            this.fetchUpdatedAboutThisItem();
        }

        onSave = (selections: Accessibility.ResourceSelections) => {

            const newGroups: Accessibility.AccResourceGroup[] = [];
            for (let group of this.itemProps.accResourceGroups) {
                const newGroup = Object.assign({}, group);
                const newResources: Accessibility.AccessibilityResource[] = [];
                for (let res of newGroup.accessibilityResources) {
                    const newRes = Object.assign({}, res);
                    newRes.currentSelectionCode = selections[newRes.resourceCode] || newRes.currentSelectionCode;
                    newResources.push(newRes);
                }
                newGroup.accessibilityResources = newResources;
                newGroups.push(newGroup);
            }

            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceGroups = newGroups;
            let cookieValue = ItemPage.toCookie(this.itemProps.accResourceGroups);
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", cookieValue, "; path=/");

            this.itemProps.currentItem = Accessibility.isBrailleEnabled(newGroups) ? this.itemProps.brailleItem : this.itemProps.nonBrailleItem;
            this.fetchUpdatedAboutThisItem();

            this.render();
        }

        onReset = () => {
            document.cookie = this.itemProps.accessibilityCookieName.concat("=", "", "; path=/");

            const newAccResourceGroups = this.itemProps.accResourceGroups.map(g => {
                const newGroup = Object.assign({}, g);
                newGroup.accessibilityResources = newGroup.accessibilityResources.map(ItemPage.resetResource);
                return newGroup;
            });
            this.itemProps.currentItem = this.itemProps.nonBrailleItem;
            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.accResourceGroups = newAccResourceGroups;

            this.itemProps.currentItem = Accessibility.isBrailleEnabled(newAccResourceGroups) ? this.itemProps.brailleItem : this.itemProps.nonBrailleItem;
            this.fetchUpdatedAboutThisItem();

            this.render();
        }

        fetchUpdatedAboutThisItem() {
            const item = this.itemProps.currentItem;

            const params = {
                bankKey: item.bankKey,
                itemKey: item.itemKey
            };

            $.ajax({
                dataType: "JSON",
                type: "GET",
                url: "/Item/AboutThisItemViewModel",
                data: params,
                success: this.onFetchedUpdatedViewModel
            });
        }

        onFetchedUpdatedViewModel = (viewModel: AboutThisItem.Props) => {
            if (!viewModel) {
                console.log("An error occurred updating the item.");
                return;
            }

            this.itemProps = Object.assign({}, this.itemProps);
            this.itemProps.aboutThisItemVM = Object.assign({}, this.itemProps.aboutThisItemVM);
            this.itemProps.aboutThisItemVM = Object.assign({}, viewModel);
            this.render();

        }

        render() {
            ReactDOM.render(
                <ItemPage.Page {...this.itemProps} onSave={this.onSave} onReset={this.onReset} />,
                this.rootDiv);
        }
    }
}