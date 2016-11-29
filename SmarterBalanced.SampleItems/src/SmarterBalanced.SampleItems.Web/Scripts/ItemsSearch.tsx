
interface SubjectClaims {
    [subject: string]: { text: string; value: string }[]
}

interface Loading {
    kind: "loading";
}

interface Success<T> {
    kind: "success";
    content: T;
}

interface Failure {
    kind: "failure";
}

interface Reloading<T> {
    kind: "reloading",
    content: T
}

/** Represents the state of an asynchronously obtained resource at a particular time. */
type Resource<T> = Loading | Success<T> | Reloading<T> | Failure

interface InteractionType {
    code: string;
    label: string;
}

namespace ItemsSearch {
    export interface Props {
        interactionTypes: InteractionType[]
        apiClient: ItemsSearchClient
    }

    export interface State {
        searchResults: Resource<ItemDigest[]>;
    }
    
    export class Component extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = { searchResults: { kind: "loading" } };

            const defaultParams: SearchAPIParams = { gradeLevels: GradeLevels.All, subjects: [], interactionTypes: [] };
            this.beginSearch(defaultParams);
        }

        beginSearch(params: SearchAPIParams) {
            const searchResults = this.state.searchResults;
            if (searchResults.kind === "success") {
                this.setState({
                    searchResults: {
                        kind: "reloading",
                        content: searchResults.content
                    }
                });
            } else if (searchResults.kind === "failure") {
                this.setState({
                    searchResults: { kind: "loading" }
                });
            }

            this.props.apiClient.itemsSearch(params, this.onSearch.bind(this), this.onError.bind(this));
        }

        onSearch(results: ItemDigest[]) {
            this.setState({ searchResults: { kind: "success", content: results } });
        }

        onError(err: any) {
            this.setState({ searchResults: { kind: "failure" } });
        }

        isLoading() {
            return this.state.searchResults.kind === "loading" || this.state.searchResults.kind === "reloading";
        }

        render() {
            const searchResults = this.state.searchResults;

            let resultsElement: JSX.Element[] | JSX.Element | undefined
            if (searchResults.kind === "success" || searchResults.kind === "reloading") {
                resultsElement = searchResults.content.length === 0
                    ? <span className="placeholder-text">No results found for the given search terms.</span>
                    : searchResults.content.map(digest => <ItemCard {...digest} />);
            } else if (searchResults.kind === "failure") {
                resultsElement = <div className="placeholder-text">An error occurred. Please try again later.</div>;
            } else {
                resultsElement = undefined;
            }
            const isLoading = this.isLoading();
            return (
                <div className="search-container">
                    <ItemSearchParams.Component
                        interactionTypes={this.props.interactionTypes}
                        onChange={(params) => this.beginSearch(params)}
                        isLoading={isLoading} />
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
    gradeLevels: GradeLevels;
    subjects: string[];
    interactionTypes: string[];
}

function initializeItemsSearch(interactionTypes: any) {
    ReactDOM.render(
        <ItemsSearch.Component apiClient={client}
            interactionTypes={interactionTypes} />,
        document.getElementById("search-container") as HTMLElement);
}
