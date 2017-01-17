
interface ItemCardViewModel {
    bankKey: number;
    itemKey: number;
    title: string;
    grade: GradeLevels;
    gradeLabel: string;
    subjectCode: string;
    subjectLabel: string;
    claimCode: string;
    claimLabel: string;
    target: string;
    interactionTypeCode: string;
    interactionTypeLabel: string;
    commonCoreStandardsId: string;
}

function itemPageLink(bankKey: number, itemKey: number) {
    window.location.href = "/Item/Details?bankKey=" + bankKey + "&itemKey=" + itemKey;
}

class ItemCard extends React.Component<ItemCardViewModel, {}> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div className={"card card-block " + this.props.subjectCode.toLowerCase()} onClick={e => itemPageLink(bankKey, itemKey)}>
                <div className="card-contents">
                    <h4 className="card-title" tabIndex={0}>{this.props.title}</h4>
                    <p className="card-text subject" tabIndex={0}>
                        <span className="card-text-label">Subject:</span>
                        <span className="card-text-value"> {this.props.subjectLabel}</span>
                    </p>
                    <p className="card-text grade" tabIndex={0}>
                        <span className="card-text-label">Grade:</span>
                        <span className="card-text-value"> {this.props.gradeLabel}</span>
                    </p>
                    <p className="card-text claim" tabIndex={0}>
                        <span className="card-text-label">Claim:</span>
                        <span className="card-text-value"> {this.props.claimLabel}</span>
                    </p>
                    <p className="card-text target" tabIndex={0}>
                        <span className="card-text-label">Target:</span>
                        <span className="card-text-value"> {this.props.target}</span>
                    </p>
                    <p className="card-text interaction-type" tabIndex={0}>
                        <span className="card-text-label">Item Type:</span>
                        <span className="card-text-value"> {this.props.interactionTypeLabel}</span>
                    </p>
                    <p className="card-text item-id" tabIndex={0}>
                        <span className="card-text-label">Item Id:</span>
                        <span className="card-text-value"> {this.props.itemKey}</span>
                    </p>
                </div>
            </div>
        );
    }
}

