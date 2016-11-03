
interface SubjectClaims {
    [subject: string]: { text: string; value: string }[]
}

interface Success<T> {
    kind: "success";
    content: T;
}

interface Failure<T> {
    kind: "failure";
    content: string;
}

interface Loading {
    kind: "loading";
}

/** Represents the state of an asynchronously obtained resource at a particular time. */
type Resource<T> = Loading | Success<T> | Failure<T>

namespace ItemsSearch {
    export interface Props {
        subjectClaims: SubjectClaims
        apiClient: ItemsSearchClient
    }

    export interface State {
        searchResults: Resource<ItemDigest[]>;
    }
    
    export class Component extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = { searchResults: { kind: "loading" } };

            const defaultParams: SearchAPIParams = { terms: "", gradeLevels: GradeLevels.All, subjects: [], interactionTypes: [] };
            this.beginSearch(defaultParams);
        }

        beginSearch(params: SearchAPIParams) {
            this.props.apiClient.itemsSearch(params, this.onSearch.bind(this), this.onError.bind(this));
        }

        onSearch(results: ItemDigest[]) {
            this.setState({ searchResults: { kind: "success", content: results } });
        }

        onError(err: any) {
            console.log(err);
            this.setState({ searchResults: { kind: "failure", content: err } });
        }

        render() {
            const searchResults = this.state.searchResults;

            let resultsElement: JSX.Element[] | JSX.Element
            if (searchResults.kind === "success") {
                resultsElement = searchResults.content.length === 0
                    ? <span className="placeholder-text">No results found for the given search terms.</span>
                    : searchResults.content.map(digest => <ItemCard {...digest} />);
            } else if (searchResults.kind === "failure") {
                resultsElement = <span className="placeholder-text">An error occurred. Please try again later.</span>;
            } else {
                resultsElement = <span></span>;
            }
             
            return (
                <div className="search-container">
                    <ItemSearchParams.Component subjectInteractionTypes={this.props.subjectClaims} onChange={(params) => this.beginSearch(params)} />
                    <div className="search-results">
                        {resultsElement}
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

// TODO: this is used to support showing/hiding or enabling/disabling tags based on whether they are associated with other tags.
// Would it be better to just give an empty result set for a mutually exclusive set of tags?
const subjectInteractionTypes = {
    "ELA": [{ text: "Reading", value: "R" }],
    "MATH": []
};

ReactDOM.render(
    <ItemsSearch.Component apiClient={client}
        subjectClaims={subjectInteractionTypes} />,
    document.getElementById("search-container") as HTMLElement);
