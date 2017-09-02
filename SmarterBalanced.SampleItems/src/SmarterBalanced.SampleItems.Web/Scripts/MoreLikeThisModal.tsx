﻿import * as React from 'react';
import { ItemCardViewModel, ItemCardCondensed } from './ItemCard'

export interface Column {
    label: string;
    itemCards: ItemCardViewModel[];
}

export interface Props {
    gradeBelowItems: Column | null;
    sameGradeItems: Column;
    gradeAboveItems: Column | null;
}

export class Modal extends React.Component<Props, {}> {

    constructor(props: Props) {
        super(props)
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e: React.KeyboardEvent<HTMLButtonElement>) {
        if ($('#modalCloseFirst-More').is(":focus") && (e.shiftKey && e.keyCode == 9)) {
            e.preventDefault();
            $('#modalCloseLast-More').focus();
        } else if ($('#modalCloseLast-More').is(":focus") && (e.which || e.keyCode == 9)) {
            e.preventDefault();
            $('#modalCloseFirst-More').focus();
        }
    }

    renderColumn(column: Column | null) {
        if (!column || column.label == "NA") {
            return undefined;
        }

        const noneLabel = "No items found for this grade.";

        const items = column.itemCards.length ?
            column.itemCards.map(c => <ItemCardCondensed key={c.bankKey + "-" + c.itemKey} {...c} />)
            : noneLabel;

        return (
            <div className="more-like-this-column">
                <div><h3>{column.label}</h3></div>
                {items}
            </div>
        );
    }

    render() {
        return (
            <div className="modal fade" id="more-like-this-modal-container" tabIndex={-1} role="dialog" aria-labelledby="More Like This Modal" aria-hidden="true">
                <div className="modal-dialog more-like-this-modal" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" id="modalCloseFirst-More" onKeyDown={this.handleChange} aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h3 className="modal-title" id="myModalLabel">More Like This</h3>
                        </div>
                        <div className="modal-body">
                            <br />
                            <div className="more-like-this">
                                {this.renderColumn(this.props.gradeBelowItems)}
                                {this.renderColumn(this.props.sameGradeItems)}
                                {this.renderColumn(this.props.gradeAboveItems)}
                            </div>        
                        </div>
                        <div className="modal-footer">
                            <button className="btn btn-primary" data-dismiss="modal" onKeyDown={this.handleChange} id="modalCloseLast-More">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            );
    }
}
