
interface ItemDigest {
    title: string;
    bankKey: number;
    itemKey: number;
    subjectId: string;
    grade: GradeLevels;
    displayGrade: string;
    claimId: string;
    targetId: string;
    interactionTypeLabel: string;
    associatedStimulus: number | null;
    subject: Subject;
    claim: Claim;
}

interface Subject {
    code: string;
    shortLabel: string;
    label: string;
}

interface Claim {
    code: string;
    claimNumber: string;
    label: string;
}

function itemPageLink(bankKey: number, itemKey: number) {
    window.location.href = "/Item/Details?bankKey=" + bankKey + "&itemKey=" + itemKey;
}

class ItemCard extends React.Component<ItemDigest, {}> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div className={'card card-block ' + this.props.subjectId.toLowerCase()} onClick={e => itemPageLink(bankKey, itemKey)}>
                <div className="card-contents">
                    <h4 className="card-title">{this.props.title}</h4>
                    <p className="card-text subject">
                        <span className="card-text-label">Subject:</span>
                        <span className="card-text-value"> {this.props.subject.shortLabel}</span>
                    </p>
                    <p className="card-text grade">
                        <span className="card-text-label">Grade:</span>
                        <span className="card-text-value"> {this.props.displayGrade}</span>
                    </p>
                    <p className="card-text claim">
                        <span className="card-text-label">Claim:</span>
                        <span className="card-text-value"> {this.props.claim.label}</span>
                    </p>
                    <p className="card-text target">
                        <span className="card-text-label">Target:</span>
                        <span className="card-text-value"> {this.props.targetId}</span>
                    </p>
                    <p className="card-text interaction-type">
                        <span className="card-text-label">Interaction Type:</span>
                        <span className="card-text-value"> {this.props.interactionTypeLabel}</span>
                    </p>
                    <p className="card-text item-id">
                        <span className="card-text-label">Item Id:</span>
                        <span className="card-text-value"> {this.props.itemKey}</span>
                    </p>
                </div>
            </div>
        );
    }
}
