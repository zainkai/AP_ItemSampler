﻿/// <reference types="google.analytics" />

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as Accessibility from '../Accessibility/Accessibility';
import * as AccessibilityModal from '../Accessibility/AccessibilityModal';
import * as Dropdown from '../Accessibility/DropDown';
import * as MoreLikeThis from '../Modals/MoreLikeThisModal';
import * as AboutThisItem from '../AboutItem/AboutThisItem';
import * as AboutPT from '../PerformanceType/AboutPT';
import * as AboutPTPopup from '../PerformanceType/AboutPTPopup';
import * as Braille from '../Accessibility/Braille';
import * as Share from '../Modals/ShareModal';
import { ItemFrame } from '../AboutItem/ItemViewerFrame';
import * as $ from 'jquery';

function toiSAAP(accResourceGroups: Accessibility.AccResourceGroup[]): string {
    let isaapCodes = "TDS_ITM1;TDS_APC_SCRUBBER;"; // always enable item tools menu
    for (let group of accResourceGroups) {
        for (let res of group.accessibilityResources) {
            if (res.currentSelectionCode && !res.disabled) {
                isaapCodes += res.currentSelectionCode + ";";
            }
        }
    }

    return encodeURIComponent(isaapCodes);
}

export function resetResource(model: Accessibility.AccessibilityResource): Accessibility.AccessibilityResource {
    const newModel = Object.assign({}, model);
    newModel.currentSelectionCode = model.defaultSelection;
    return newModel;
}

function trimAccResource(resource: Accessibility.AccessibilityResource): { label: string, selectedCode: string } {
    return {
        label: resource.label,
        selectedCode: resource.currentSelectionCode,
    };
}

export function toCookie(accGroups: Accessibility.AccResourceGroup[]): string {
    let prefs: Accessibility.ResourceSelections = {};
    for (const group of accGroups) {
        for (const resource of group.accessibilityResources) {
            prefs[resource.resourceCode] = resource.currentSelectionCode;
        }
    }

    const json = JSON.stringify(prefs);
    const cookie = btoa(json);
    return cookie;
}

function addDisabledPlaceholder(resource: Accessibility.AccessibilityResource): Accessibility.AccessibilityResource {
    if (resource.disabled) {
        let newSelection = { ...resource };
        let disabledOption: Dropdown.Selection = {
            label: "Disabled for item",
            selectionCode: "",
            disabled: true,
            order: 0,
            hidden: false
        };
        newSelection.selections.push(disabledOption);
        newSelection.currentSelectionCode = "";
        return newSelection;
    }
    return resource; function readCookie(name: string): string | undefined {
        var cookie = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
        return cookie ? cookie.pop() : '';
    }
}

export interface ItemIdentifier {
    itemName: string;
    bankKey: number;
    itemKey: number;
}

export interface ViewModel {
    itemViewerServiceUrl: string;
    itemNames: string;
    brailleItemNames: string;
    brailleItem: ItemIdentifier;
    nonBrailleItem: ItemIdentifier;
    currentItem: ItemIdentifier;
    accessibilityCookieName: string;
    isPerformanceItem: boolean;
    performanceItemDescription: string;
    subject: string;
    accResourceGroups: Accessibility.AccResourceGroup[];
    moreLikeThisVM: MoreLikeThis.Props;
    brailleItemCodes: string[];
    braillePassageCodes: string[];

}

export interface Props extends ViewModel {
    onSave: (selections: Accessibility.ResourceSelections) => void;
    onReset: () => void;
    aboutThisItemVM: AboutThisItem.Props;
}

interface State { }

export class Page extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
    }

    saveOptions = (resourceSelections: Accessibility.ResourceSelections): void => {
        this.props.onSave(resourceSelections);
    }

    openAboutItemModal(e: React.KeyboardEvent<HTMLAnchorElement>) {
        if (e.keyCode === 13 || e.keyCode === 23 || e.keyCode === 32) {
            const modal: any = ($("#about-item-modal-container"));
            modal.modal();
        }
    }

    openMoreLikeThisModal(e: React.KeyboardEvent<HTMLAnchorElement>) {
        if (e.keyCode === 13 || e.keyCode === 23 || e.keyCode === 32) {
            const modal: any = ($("#more-like-this-modal-container"));
            modal.modal();
        }
    }

    openShareModal(e: React.KeyboardEvent<HTMLAnchorElement>) {
        if (e.keyCode === 13 || e.keyCode === 23 || e.keyCode === 32) {
            const modal: any = ($("#share-modal-container"));
            modal.modal();
        }
    }

    openPerfTaskModal(e: React.KeyboardEvent<HTMLAnchorElement>) {
        if (e.keyCode === 13 || e.keyCode === 23 || e.keyCode === 32) {
            const modal: any = ($("#about-performance-tasks-modal-container"));
            modal.modal();
        }
    }

    openAccessibilityModal(e: React.KeyboardEvent<HTMLButtonElement>) {
        if (e.keyCode === 13 || e.keyCode === 23 || e.keyCode === 32) {
            const modal: any = ($("#accessibility-modal-container"));
            modal.modal();
        }
    }

    renderPerformanceItemModalBtn = (isPerformanceItem: boolean) => {
        if (!isPerformanceItem) {
            return undefined;
        }

        let btnText = (
            <span>
                <span className="item-nav-long-label">This is a </span><b>Performance Task</b>
            </span>
        );

        return (
            <a className="item-nav-btn" data-toggle="modal" data-target="#about-performance-tasks-modal-container"
                onKeyUp={e => this.openPerfTaskModal(e)} role="button" tabIndex={0}>
                <span className="glyphicon glyphicon-info-sign glyphicon-pad" aria-hidden="true" />
                {btnText}
            </a>
        );
    }

    renderModals(): JSX.Element {
        const abtText = <span>About <span className="item-nav-long-label">This Item</span></span>;
        const moreText = <span>More <span className="item-nav-long-label">Like This</span></span>;


        return (
            <div className="item-nav" role="toolbar" aria-label="Toolbar with button groups">
                <div className="item-nav-left-group" role="group" aria-label="First group">
                    <a className="item-nav-btn" data-toggle="modal" data-target="#about-item-modal-container"
                        onKeyUp={e => this.openAboutItemModal(e)} role="button" tabIndex={0}>
                        <span className="glyphicon glyphicon-info-sign glyphicon-pad" aria-hidden="true" />
                        {abtText}
                    </a>
                    <a className="item-nav-btn" data-toggle="modal" data-target="#more-like-this-modal-container"
                        onKeyUp={e => this.openMoreLikeThisModal(e)} role="button" tabIndex={0}>
                        <span className="glyphicon glyphicon-th-large glyphicon-pad" aria-hidden="true" />
                        {moreText}
                    </a>
                    <a className="item-nav-btn" data-toggle="modal" data-target="#share-modal-container"
                        onKeyUp={e => this.openShareModal(e)} role="button" tabIndex={0}>
                        <span className="glyphicon glyphicon-share-alt glyphicon-pad" aria-hidden="true" />
                        Share
                        </a>
                    {this.renderPerformanceItemModalBtn(this.props.isPerformanceItem)}
                    <Braille.BrailleLink
                        currentSelectionCode={Accessibility.getBrailleAccommodation(this.props.accResourceGroups)}
                        brailleItemCodes={this.props.brailleItemCodes}
                        braillePassageCodes={this.props.braillePassageCodes}
                        bankKey={this.props.currentItem.bankKey}
                        itemKey={this.props.currentItem.itemKey} />
                </div>
                {this.renderModalFrames()}
                {this.renderAccessabilityBtn()}
            </div>
        );
    }

    renderAccessabilityBtn(): JSX.Element {
        return (
            <div className="item-nav-right-group" role="group" aria-label="Second group">
                <button className="accessibility-btn btn btn-primary" data-toggle="modal"
                    data-target="#accessibility-modal-container"
                    onClick={e => ga("send", "event", "button", "OpenAccessibility")}
                    onKeyUp={e => this.openAccessibilityModal(e)} tabIndex={0}>
                    <span className="glyphicon glyphicon-collapse-down" aria-hidden="true"></span>
                    Accessibility
                    </button>
            </div>
        );
    }

    renderModalFrames(): JSX.Element {
        let isaap = toiSAAP(this.props.accResourceGroups);
        return (
            <div>
                <AboutThisItem.ATIComponent {...this.props.aboutThisItemVM} />
                <AccessibilityModal.ItemAccessibilityModal
                    accResourceGroups={this.props.accResourceGroups}
                    onSave={this.props.onSave}
                    onReset={this.props.onReset} />
                <MoreLikeThis.Modal {...this.props.moreLikeThisVM} />
                <Share.ShareModal iSAAP={isaap} />
                <AboutPTPopup.Modal subject={this.props.subject} description={this.props.performanceItemDescription} isPerformance={this.props.isPerformanceItem} />
                <AboutPT.Modal subject={this.props.subject} description={this.props.performanceItemDescription} />
            </div>
        );
    }

    renderIFrame(): JSX.Element {
        let isaap = toiSAAP(this.props.accResourceGroups);
        const itemNames = (Accessibility.isBrailleEnabled(this.props.accResourceGroups)) ? this.props.brailleItemNames : this.props.itemNames;
        let scrollTo: string = Accessibility.isStreamlinedEnabled(this.props.accResourceGroups) ? "" : ("&scrollToId=").concat(this.props.currentItem.itemName);
        let ivsUrl: string = this.props.itemViewerServiceUrl.concat("/items?ids=", itemNames, "&isaap=", isaap, scrollTo);

        return (<ItemFrame url={ivsUrl} />);
    }

    render() {
        return (
            <div>
                {this.renderModals()}
                {this.renderIFrame()}
            </div >
        );
    }
}


