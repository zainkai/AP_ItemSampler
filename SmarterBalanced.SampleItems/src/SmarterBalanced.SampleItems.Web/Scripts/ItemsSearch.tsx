// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

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

interface SearchParams {
    terms?: string;
    gradeLevels?: GradeLevels;
    subjects?: string[];
    claimType?: string;
}

class ItemsSearchForm extends React.Component<{}, SearchParams> {
    constructor(props: {}) {
        super(props);
        this.state = {
            terms: "",
            gradeLevels: GradeLevels.All,
            subjects: [],
            claimType: ""
        };
    }

    toggleGrades(grades: GradeLevels) {
        this.setState({
            // Exclusive OR to flip just the bits for the input grades
            gradeLevels: this.state.gradeLevels ^ grades
        });
    }

    toggleElementary() {
    }

    render() {
        const elementarySelected = (this.state.gradeLevels & GradeLevels.Elementary) == GradeLevels.Elementary;
        const middleSelected = (this.state.gradeLevels & GradeLevels.Middle) == GradeLevels.Middle;
        const highSelected = (this.state.gradeLevels & GradeLevels.High) == GradeLevels.High;

        return (
            <form>
                <label htmlFor="terms">Terms</label>
                <input name="terms"
                    value={this.state.terms}
                    onChange={e => this.setState({ terms: (e.target as HTMLInputElement).value })} />

                <label htmlFor="grade-levels">Grade Levels</label>
                <div className={(elementarySelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.Elementary)}>

                    Elementary School
                </div>

                <div className={(middleSelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.Middle)}>

                    Middle School
                </div>

                <div className={(highSelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.High)}>

                    High School
                </div>

                <label htmlFor="subjects">Subjects</label>
                <input name="terms"
                    value={this.state.terms}
                    onChange={e => this.setState({ terms: (e.target as HTMLInputElement).value })} />

                <label htmlFor="interaction-types">Interaction Types</label>
                <input name="terms"
                    value={this.state.terms}
                    onChange={e => this.setState({ terms: (e.target as HTMLInputElement).value })} />


            </form>
        );
    }
}

interface ItemsSearchProps {
    searchResults: ItemDigest[]
}

class ItemsSearch extends React.Component<ItemsSearchProps, {}> {
    render() {
        const itemCards = this.props.searchResults.map(digest => <ItemCard {...digest} />);
        return (
            <div>
                <ItemsSearchForm />
                {itemCards}
            </div>
        );
    }
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
    ReactDOM.render(<ItemsSearch searchResults={data} />, document.getElementById("container") as HTMLElement);
}