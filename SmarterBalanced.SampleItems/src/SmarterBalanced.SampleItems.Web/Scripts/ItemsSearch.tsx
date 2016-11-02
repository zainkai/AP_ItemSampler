
interface SubjectClaims {
    [subject: string]: { text: string; value: string }[]
}

namespace ItemsSearch {
    export interface Props {
        subjectClaims: SubjectClaims
        apiClient: ItemsSearchClient
    }

    export interface State {
        searchResults: ItemDigest[]
    }
    
    export class Component extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = { searchResults: [] };

            const defaultParams: SearchAPIParams = { terms: "", gradeLevels: GradeLevels.All, subjects: [], interactionTypes: [] };
            this.beginSearch(defaultParams);
        }

        beginSearch(params: SearchAPIParams) {
            this.props.apiClient.itemsSearch(params, this.onSearch.bind(this), this.onError.bind(this));
        }

        onSearch(results: ItemDigest[]) {
            this.setState({ searchResults: results });
        }

        onError(err: any) {
            console.log(err);
        }

        render() {
            const itemCards = this.state.searchResults.map(digest => <ItemCard {...digest} />);
            return (
                <div className="search-container">
                    <ItemSearchParams.Component subjectInteractionTypes={this.props.subjectClaims} onChange={(params) => this.beginSearch(params)} />
                    <div className="search-results">
                        {itemCards}
                    </div>
                </div>
            );
        }
    }
}

interface ItemsSearchClient {
    itemsSearch(params: SearchAPIParams,
        onSuccess: (data: ItemDigest[]) => void,
        onError?: (jqXHR: JQueryXHR, textStatus: string, errorThrown: string) => any): any;
}

const client: ItemsSearchClient = {
    itemsSearch: (params, onSuccess, onError) => {
        $.ajax({
            dataType: "json",
            url: "/ItemsSearch/search",
            traditional: true, // causes arrays to be serialized in a way supported by MVC
            data: params,
            success: onSuccess,
            error: onError
        });
    }
};

interface SearchAPIParams {
    terms: string;
    gradeLevels: GradeLevels;
    subjects: string[];
    interactionTypes: string[];
}

const subjectInteractionTypes = {
    "ELA": [{ text: "Reading", value: "R" }],
    "MATH": []
};

ReactDOM.render(
    <ItemsSearch.Component apiClient={client}
        subjectClaims={subjectInteractionTypes} />,
    document.getElementById("search-container") as HTMLElement);
