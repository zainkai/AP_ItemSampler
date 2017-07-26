import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as ItemCard from './ItemCard';
import * as ItemsSearchParams from './ItemsSearchParams';
import * as GradeLevels from './GradeLevels';

interface SubjectClaims {
    [subject: string]: { text: string; value: string }[];
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
    kind: "reloading";
    content: T;
}

/** Represents the state of an asynchronously obtained resource at a particular time. */
type Resource<T> = Loading | Success<T> | Reloading<T> | Failure

export interface InteractionType {
    code: string;
    label: string;
}

export interface Subject {
    code: string;
    label: string;
    claims: Claim[];
    interactionTypeCodes: string[];
}

export interface Claim {
    code: string;
    label: string;
}

namespace ItemsSearch {
    export interface Props {
        interactionTypes: InteractionType[];
        subjects: Subject[];
        apiClient: ItemsSearchClient;
    }

    export interface State {
        searchResults: Resource<ItemCard.ItemCardViewModel[]>;
    }
    
    export class ISComponent extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = { searchResults: { kind: "loading" } };
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

        onSearch(results: ItemCard.ItemCardViewModel[]) {
            this.setState({ searchResults: { kind: "success", content: results } });
        }

        onError(err: any) {
            this.setState({ searchResults: { kind: "failure" } });
        }

        selectSingleResult() {
            const searchResults = this.state.searchResults;
            if (searchResults.kind === "success" && searchResults.content.length === 1) {
                const searchResult = searchResults.content[0];
                ItemCard.itemPageLink(searchResult.bankKey, searchResult.itemKey);
            }
        }

        isLoading() {
            return this.state.searchResults.kind === "loading" || this.state.searchResults.kind === "reloading";
        }

        render() {
            const searchResults = this.state.searchResults;

            let resultsElement: JSX.Element[] | JSX.Element | undefined;
            if (searchResults.kind === "success" || searchResults.kind === "reloading") {
                resultsElement = searchResults.content.length === 0
                    ? <span className="placeholder-text" role="alert">No results found for the given search terms.</span>
                    : searchResults.content.map(digest =>
                        <ItemCard.ItemCard {...digest} key={digest.bankKey.toString() + "-" + digest.itemKey.toString()} />);
            } else if (searchResults.kind === "failure") {
                resultsElement = <div className="placeholder-text" role="alert">An error occurred. Please try again later.</div>;
            } else {
                resultsElement = undefined;
            }
            const isLoading = this.isLoading();
            return (
                <div className="search-container">
                    <ItemsSearchParams.ISPComponent
                        interactionTypes={this.props.interactionTypes}
                        subjects={this.props.subjects}
                        onChange={(params) => this.beginSearch(params)}
                        selectSingleResult={() => this.selectSingleResult()}
                        isLoading={isLoading} />
                    <div className="search-results" >
                        {resultsElement}
                    </div>
                </div>
            );
        }
    }
}

interface ItemsSearchClient {
    itemsSearch(params: SearchAPIParams,
        onSuccess: (data: ItemCard.ItemCardViewModel[]) => void,
        onError?: (jqXHR: JQueryXHR, textStatus: string, errorThrown: string) => any): any;
}

const client: ItemsSearchClient = {
    itemsSearch: (params, onSuccess, onError) => {
        $.ajax({
            dataType: "json",
            url: "/BrowseItems/search",
            traditional: true, // causes arrays to be serialized in a way supported by MVC
            data: params,
            success: onSuccess,
            error: onError
        });
    }
};

export interface SearchAPIParams {
    itemId: string;
    gradeLevels: GradeLevels.GradeLevels;
    subjects: string[];
    claims: string[];
    interactionTypes: string[];
    performanceOnly: boolean;
}

interface ItemsSearchViewModel {
    interactionTypes: InteractionType[];
    subjects: Subject[];
}

export function initializeItemsSearch(viewModel: ItemsSearchViewModel) {
    ReactDOM.render(
        <ItemsSearch.ISComponent apiClient={client} {...viewModel} />,
        document.getElementById("search-container") as HTMLElement);
}
