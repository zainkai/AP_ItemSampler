
interface ItemDigest {
    title: string;
    bankKey: number;
    itemKey: number;
    subject: string;
    subjectShortLabel: string;
    grade: GradeLevels;
    displayGrade: string;
    claimId: string;
    displayClaim: string;
    target: string;
    interactionTypeLabel: string;
    associatedStimulus: number | null;
}

function itemPageLink(bankKey: number, itemKey: number) {
    window.location.href = "/Item/Details?bankKey=" + bankKey + "&itemKey=" + itemKey;
}

class ItemCard extends React.Component<ItemDigest, {}> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div className="card card-block" onClick={e => itemPageLink(bankKey, itemKey)}>
                <div className="card-contents">
                    <h4 className="card-title">{this.props.title}</h4>
                    <p className="card-text">Claim: {this.props.displayClaim}</p>
                    <p className="card-text">Grade: {this.props.displayGrade}</p>
                    <p className="card-text">Subject: {this.props.subjectShortLabel}</p>
                    <p className="card-text">Interaction Type: {this.props.interactionTypeLabel}</p>
                </div>
            </div>
        );
    }
}
