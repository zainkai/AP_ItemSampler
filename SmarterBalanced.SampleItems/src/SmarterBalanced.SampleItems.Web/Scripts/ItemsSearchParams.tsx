
const hideArrow = (
    <span>
        <span className="screen-reader-text">Hide</span>
        <span aria-hidden="true">▼</span>
    </span>
);

const showArrow = (
    <span>
        <span className="screen-reader-text">Show</span>
        <span aria-hidden="true">▶</span>
    </span>
);

function parseQueryString(url: string): { [key: string]: string[] | undefined } {
    let queryObject: { [key: string]: string[] | undefined } = {};
    const pairs = url.slice(url.indexOf("?") + 1).split("&");
    for (const pair of pairs) {
        const pairParts = pair.split("=");
        if (pairParts[0] && pairParts[1]) {
            queryObject[pairParts[0]] = pairParts[1].split(",");
        }
    }
    return queryObject;
}

namespace ItemSearchParams {

    export interface Props {
        interactionTypes: InteractionType[];
        subjects: Subject[];
        onChange: (params: SearchAPIParams) => void;
        selectSingleResult: () => void;
        isLoading: boolean;
    }
    
    export interface State {
        itemId?: string;
        gradeLevels?: GradeLevels;
        subjects?: string[];
        claims?: string[];
        interactionTypes?: string[];

        expandItemID?: boolean;
        expandGradeLevels?: boolean;
        expandSubjects?: boolean;
        expandClaims?: boolean;
        expandInteractionTypes?: boolean;
    }

    export class ISPComponent extends React.Component<Props, State> {
        readonly initialState: State = {
            itemID: '',
            gradeLevels: GradeLevels.NA,
            subjects: [],
            claims: [],
            interactionTypes: []
        };

        // TODO: since the callback property exists on setState, should this be in the state interface instead of the component class?
        timeoutToken?: number;

        constructor(props: Props) {
            super(props);

            const queryObject = parseQueryString(location.search);
            const itemId = (queryObject["itemID"] || [])[0] || '';

            const gradeString = (queryObject["gradeLevels"] || [])[0];
            const gradeLevels: GradeLevels = parseInt(gradeString) || GradeLevels.NA;

            const subjects = queryObject["subjects"] || [];
            const claims = queryObject["claims"] || [];
            const interactionTypes = queryObject["interactionTypes"] || [];

            this.state = {
                itemId: itemId,
                gradeLevels: gradeLevels,
                subjects: subjects,
                claims: claims,
                interactionTypes: interactionTypes,

                expandItemID: itemId.length !== 0,
                expandGradeLevels: gradeLevels !== GradeLevels.NA,
                expandSubjects: subjects.length !== 0,
                expandClaims: claims.length !== 0,
                expandInteractionTypes: interactionTypes.length !== 0
            };

            this.onChange();
        }
        
        encodeQuery(): string {
            let pairs: string[] = [];
            if (this.state.claims && this.state.claims.length !== 0) {
                pairs.push("claims=" + this.state.claims.join(","));
            }
            if (this.state.gradeLevels !== GradeLevels.NA) {
                pairs.push("gradeLevels=" + this.state.gradeLevels);
            }
            if (this.state.interactionTypes && this.state.interactionTypes.length !== 0) {
                pairs.push("interactionTypes=" + this.state.interactionTypes.join(","));
            }
            if (this.state.itemId) {
                pairs.push("itemID=" + this.state.itemId);
            }
            if (this.state.subjects && this.state.subjects.length !== 0) {
                pairs.push("subjects=" + this.state.subjects.join(","));
            }

            if (pairs.length === 0) {
                return "/ItemsSearch";
            }

            const query = "?" + pairs.join("&");
            return query;
        }

        beginChangeTimeout() {
            if (this.timeoutToken !== undefined) {
                clearTimeout(this.timeoutToken);
            }

            this.timeoutToken = setTimeout(() => this.onChange(), 200);
        }

        onChange() {
            const params: SearchAPIParams = {
                itemId: this.state.itemId || '',
                gradeLevels: this.state.gradeLevels || GradeLevels.All,
                subjects: this.state.subjects || [],
                claims: this.state.claims || [],
                interactionTypes: this.state.interactionTypes || []
            };
            this.props.onChange(params);
        }

        onItemIDInput(e: React.FormEvent) {
            const newValue = (e.target as HTMLInputElement).value;
            const isInputOK = /^\d{0,4}$/.test(newValue);
            if (isInputOK) {
                this.setState({
                    itemId: newValue
                }, () => this.beginChangeTimeout());
            }
        }

        onItemIDKeyUp(e: React.KeyboardEvent) {
            if (e.keyCode === 13) {
                this.props.selectSingleResult();
            }
        }

        toggleGrades(grades: GradeLevels) {
            this.setState({
                // Exclusive OR to flip just the bits for the input grades
                gradeLevels: this.state.gradeLevels ^ grades
            }, () => this.beginChangeTimeout());

        }

        toggleSubject(subject: string) {
            const subjectCodes = this.state.subjects || [];
            const containsSubject = subjectCodes.indexOf(subject) !== -1;
            const newSubjectCodes = containsSubject ? subjectCodes.filter(s => s !== subject) : subjectCodes.concat([subject]);
            
            if (newSubjectCodes.length === 0) {
                this.setState({
                    subjects: newSubjectCodes,
                    claims: [],
                    interactionTypes: []
                }, () => this.beginChangeTimeout());
                return;
            }

            const newSubjects = this.props.subjects.filter(s => newSubjectCodes.indexOf(s.code) !== -1);
            
            // Remove all claims not contained by the newly selected subjects
            const subjectClaimCodes = newSubjects.reduce((prev: string[], cur: Subject) => prev.concat(cur.claims.map(c => c.code)), []);
            const newClaimCodes = (this.state.claims || []).filter(c => subjectClaimCodes.indexOf(c) !== -1);

            const subjectInteractionCodes = newSubjects.reduce((prev: string[], cur: Subject) => prev.concat(cur.interactionTypeCodes), []);
            const newInteractionCodes = (this.state.interactionTypes || []).filter(i => subjectInteractionCodes.indexOf(i) !== -1);

            this.setState({
                subjects: newSubjectCodes,
                claims: newClaimCodes,
                interactionTypes: newInteractionCodes
            }, () => this.beginChangeTimeout());
        }

        toggleClaim(claim: string) {
            const claims = this.state.claims || [];
            const containsClaim = claims.indexOf(claim) !== -1;
            this.setState({
                claims: containsClaim ? claims.filter(c => c !== claim) : claims.concat([claim])
            }, () => this.beginChangeTimeout());
        }

        toggleInteractionType(code: string) {
            const interactionTypes = this.state.interactionTypes || [];
            const containsSubject = interactionTypes.indexOf(code) !== -1;
            this.setState({
                interactionTypes: containsSubject ? interactionTypes.filter(s => s !== code) : interactionTypes.concat([code])
            }, () => this.beginChangeTimeout());
        }

        /**
         * Returns a value indicating whether all search categories are expanded.
         */
        getExpandAll() {
            const { expandItemID, expandGradeLevels, expandSubjects, expandClaims, expandInteractionTypes } = this.state;
            const expandAll = expandItemID && expandGradeLevels && expandSubjects && expandClaims && expandInteractionTypes;
            return expandAll;
        }

        toggleExpandItemIDInput() {
            this.setState({
                expandItemID: !this.state.expandItemID
            });
        }

        toggleExpandGradeLevels() {
            this.setState({
                expandGradeLevels: !this.state.expandGradeLevels
            });
        }

        toggleExpandSubjects() {
            this.setState({
                expandSubjects: !this.state.expandSubjects
            });
        }

        toggleExpandClaims() {
            this.setState({
                expandClaims: !this.state.expandClaims
            });
        }

        toggleExpandInteractionTypes() {
            this.setState({
                expandInteractionTypes: !this.state.expandInteractionTypes
            });
        }

        toggleExpandAll() {
            // If everything is already expanded, then collapse everything. Otherwise, expand everything.
            const expandAll = !this.getExpandAll();
            this.setState({
                expandItemID: expandAll,
                expandGradeLevels: expandAll,
                expandSubjects: expandAll,
                expandClaims: expandAll,
                expandInteractionTypes: expandAll
            });
        }

        resetFilters() {
            this.setState(this.initialState, () => this.beginChangeTimeout());
        }

        render() {
            history.replaceState(null, '', this.encodeQuery());

            return (
                <div className="search-params">
                    <div className="search-header">
                        <h1 className="search-title" tabIndex={0}>Sample Items Search</h1>
                        <div className="search-status">
                            {this.props.isLoading ? <img src="images/spin.gif" className="spin" /> : undefined}
                            <div><a onClick={() => this.resetFilters()} tabIndex={0}>Reset filters</a></div>
                            <div onClick={() => this.toggleExpandAll()} tabIndex={0}>
                                {this.getExpandAll() ? "▼ Hide" : "▶ Show"} all
                            </div>
                        </div>
                    </div>
                    <div className="search-categories">
                        {this.renderGrades()}
                        {this.renderSubjects()}
                        {this.renderClaims()}
                        {this.renderInteractionTypes()}
                        {this.renderItemID()}
                    </div>
                </div>
            );
        }

        renderItemID() {
            const input = this.state.expandItemID
                ?
                    <input type="text" className="form-control"
                        placeholder="Item ID"
                        onChange={e => this.onItemIDInput(e)}
                        onKeyUp={e => this.onItemIDKeyUp(e)}
                        value={this.state.itemId}>
                    </input>
                : undefined;

            return (
                <div className="search-category">
                    <label aria-expanded={this.state.expandItemID} onClick={() => this.toggleExpandItemIDInput()} tabIndex={0}>
                        {this.state.expandItemID ? hideArrow : showArrow} Item ID
                    </label>
                    {input}
                </div>
            );
        }

        renderGrades() {
            const gradeLevels = this.state.gradeLevels || GradeLevels.NA;
            const elementarySelected = GradeLevels.contains(gradeLevels, GradeLevels.Elementary);
            const middleSelected = GradeLevels.contains(gradeLevels, GradeLevels.Middle);
            const highSelected = GradeLevels.contains(gradeLevels, GradeLevels.High);

            const tags = [
                <button role="button" key={GradeLevels.Elementary} className={(elementarySelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.Elementary)}
                    tabIndex={0}
                    aria-pressed={elementarySelected}
                    aria-label="Grades 3 to 5">

                    Grades 3-5
                </button>,

                <button role="button" key={GradeLevels.Middle} className={(middleSelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.Middle)}
                    tabIndex={0}
                    aria-pressed={middleSelected}
                    aria-label="Grades 6 to 8">

                    Grades 6-8
                </button>,

                <button role="button" key={GradeLevels.High} className={(highSelected ? "selected" : "") + " tag"}
                    onClick={() => this.toggleGrades(GradeLevels.High)}
                    tabIndex={0}
                    aria-pressed={highSelected}>

                    High School
                </button>
            ];

            // TODO: convert to use Collapsible element
            return (
                <div className="search-category" style={{ flexGrow: 3 }}>
                    <label aria-expanded={this.state.expandGradeLevels}
                        onClick={() => this.toggleExpandGradeLevels()}
                        tabIndex={0}>

                        {this.state.expandGradeLevels ? hideArrow : showArrow} Grade Levels
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandGradeLevels ? tags : undefined}
                    </div>
                </div>
            );
        }

        renderSubject(subject: Subject) {
            const subjects = this.state.subjects || [];
            const containsSubject = subjects.indexOf(subject.code) !== -1;
            const className = (containsSubject ? "selected" : "") + " tag";
            return (
                <button role="button" key={subject.code} className={className}
                    onClick={() => this.toggleSubject(subject.code)}
                    tabIndex={0}
                    aria-pressed={containsSubject}>

                    {subject.label}
                </button>
            );
        }

        renderSubjects() {
            const subjects = this.state.subjects || [];
            const tags = this.state.expandSubjects
                ? this.props.subjects.map(s => this.renderSubject(s))
                : undefined;

            return (
                <div className="search-category" style={{ flexGrow: 2 }}>
                    <label aria-expanded={this.state.expandSubjects}
                        onClick={() => this.toggleExpandSubjects()}
                        tabIndex={0}>

                        {this.state.expandSubjects ? hideArrow : showArrow} Subjects
                    </label>
                    <div className="search-tags form-group">
                        {tags}
                    </div>
                </div>
            );
        }

        renderClaims() {
            const selectedClaims = this.state.claims || [];
            
            const renderClaim = (claim: Claim) => {
                let containsClaim = selectedClaims.indexOf(claim.code) !== -1;
                return (
                    <button role="button" key={claim.code} className={(containsClaim ? "selected" : "") + " tag"}
                        onClick={() => this.toggleClaim(claim.code)}
                        tabIndex={0}
                        aria-pressed={containsClaim}>

                        {claim.label}
                    </button>
                );
            }

            // If no subjects are selected, use the entire list of subjects
            const selectedSubjectCodes = this.state.subjects || [];
            const subjects = selectedSubjectCodes.length !== 0
                ? this.props.subjects.filter(s => selectedSubjectCodes.indexOf(s.code) !== -1)
                : [];

            const tags = subjects.length === 0
                ? <p tabIndex={0}>Please select a subject.</p>
                : subjects
                    .reduce((cs: Claim[], s: Subject) => cs.concat(s.claims), [])
                    .map(renderClaim);

            return (
                <div className="search-category" style={{ flexGrow: this.props.subjects.length }}>
                    <label aria-expanded={this.state.expandClaims}
                        onClick={() => this.toggleExpandClaims()}
                        tabIndex={0}>

                        {this.state.expandClaims ? hideArrow : showArrow} Claims
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandClaims ? tags : undefined}
                    </div>
                </div>
            );
        }

        renderInteractionTypes() {
            const selectedInteractionTypes = this.state.interactionTypes || [];
            
            const renderInteractionType = (it: InteractionType) => {
                let containsInteractionType = selectedInteractionTypes.indexOf(it.code) !== -1;
                return (
                    <button key={it.code} className={(containsInteractionType ? "selected" : "") + " tag"}
                        onClick={() => this.toggleInteractionType(it.code)}
                        tabIndex={0}
                        aria-pressed={containsInteractionType}>

                        {it.label}
                    </button>
                );
            }
            
            const selectedSubjectCodes = this.state.subjects || [];
            const selectedSubjects = selectedSubjectCodes.length !== 0
                ? this.props.subjects.filter(subj => selectedSubjectCodes.indexOf(subj.code) !== -1)
                : [];

            const visibleInteractionTypes = selectedSubjects.length !== 0
                ? this.props.interactionTypes.filter(it => selectedSubjects.some(subj => subj.interactionTypeCodes.indexOf(it.code) !== -1))
                : [];

            const tags = visibleInteractionTypes.length === 0
                ? <p tabIndex={0}>Please first select a subject.</p>
                : visibleInteractionTypes.map(renderInteractionType);

            return (
                <div className="search-category" style={{ flexGrow: this.props.interactionTypes.length }}>
                    <label aria-expanded={this.state.expandInteractionTypes}
                        onClick={() => this.toggleExpandInteractionTypes()}
                        tabIndex={0}>

                        {this.state.expandInteractionTypes ? hideArrow : showArrow} Interaction Types
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandInteractionTypes ? tags : undefined}
                    </div>
                </div>
            );
        }
    }
}
