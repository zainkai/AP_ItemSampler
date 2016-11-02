
interface ItemDigest {
    bankKey: number;
    itemKey: number;
    subject: string;
    grade: GradeLevels;
    claim: string | null;
    target: string;
    interactionType: string;
    associatedStimulus: number | null;
}

function itemPageLink(bankKey: number, itemKey: number) {
    window.location.href = "/Item/Details?bankKey=" + bankKey + "&itemKey=" + itemKey;
}

class ItemCard extends React.Component<ItemDigest, {}> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div key={bankKey.toString() + "-" + itemKey.toString()} className="card card-block" onClick={e => itemPageLink(bankKey, itemKey)}>
                <div className="card-contents">
                    <h4 className="card-title">{bankKey}-{itemKey}</h4>
                    <p className="card-text">Claim: {this.props.claim}</p>
                    <p className="card-text">Grade: {GradeLevels.toString(this.props.grade)}</p>
                    <p className="card-text">Subject: {this.props.subject}</p>
                    <p className="card-text">Interaction Type: {this.props.interactionType}</p>
                </div>
            </div>
        );
    }
}
