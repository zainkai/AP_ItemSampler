// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

interface SearchParams {
    terms: string;
    gradeLevels: GradeLevels;
    subjects: string[];
    claimType: string;
}

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

class ItemCard extends React.Component<ItemDigest, Object> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div className="card card-block" onClick={e => itemPageLink(bankKey, itemKey)}>
                <div className="card-contents">
                    <h4 className="card-title">{bankKey}-{itemKey}</h4>
                    <p className="card-text">Claim: {this.props.claim}</p>
                    <p className="card-text">Grade: {this.props.grade}</p>
                    <p className="card-text">Subject: {this.props.subject}</p>
                    <p className="card-text">Interaction Type: {this.props.interactionType}</p>
                </div>
            </div>
        );
    }
}

const data = { terms: "foo", gradeLevels: GradeLevels.Elementary, subjects: ["ELA"], claimType: "" };

$.ajax({
    dataType: "json",
    url: "/ItemsSearch/search",
    traditional: true, // causes arrays to be serialized in a way supported by MVC
    data: data,
    success: onSearch
});

function onSearch(data: ItemDigest[]) {
    const itemCards = data.map(digest => <ItemCard {...digest} />);
    ReactDOM.render(<div className="container">{itemCards}</div>, document.getElementById("container") as HTMLElement);
}