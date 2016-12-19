
namespace ItemSearchParams {

    export interface Props {
        interactionTypes: InteractionType[];
        subjects: Subject[];
        onChange: (params: SearchAPIParams) => void;
        isLoading: boolean;
    }

    export interface State {
        gradeLevels?: GradeLevels;
        subjects?: string[];
        claims?: string[];
        interactionTypes?: string[];
        
        expandGradeLevels?: boolean;
        expandSubjects?: boolean;
        expandClaims?: boolean;
        expandInteractionTypes?: boolean;
    }

    export class Component extends React.Component<Props, State> {
        readonly initialState: State = {
            gradeLevels: GradeLevels.NA,
            subjects: [],
            claims: [],
            interactionTypes: []
        };

        // TODO: since the callback property exists on setState, should this be in the state interface instead of the component class?
        timeoutToken?: number;

        constructor(props: Props) {
            super(props);
            this.state = this.initialState;
        }

        beginChangeTimeout() {
            if (this.timeoutToken !== undefined) {
                clearTimeout(this.timeoutToken);
            }

            this.timeoutToken = setTimeout(() => {
                const params: SearchAPIParams = {
                    gradeLevels: this.state.gradeLevels || GradeLevels.All,
                    subjects: this.state.subjects || [],
                    claims: this.state.claims || [],
                    interactionTypes: this.state.interactionTypes || []
                };
                this.props.onChange(params);
            }, 200);
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

            // No filtering needed if no subjects are selected
            if (newSubjectCodes.length === 0) {
                this.setState({
                    subjects: newSubjectCodes
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
            const { expandGradeLevels, expandSubjects, expandClaims, expandInteractionTypes } = this.state;
            const expandAll = expandGradeLevels && expandSubjects && expandClaims && expandInteractionTypes;
            return expandAll;
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
            const { expandGradeLevels, expandSubjects, expandClaims, expandInteractionTypes } = this.state;
            // If everything is already expanded, then collapse everything. Otherwise, expand everything.
            const expandAll = !this.getExpandAll();
            this.setState({
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
            return (
                <div className="search-params">
                    <div className="search-header">
                        <div className="search-title">Sample Items Search</div>
                        <div className="search-status">
                            {this.props.isLoading ? <img src="images/spin.gif" className="spin"/> : undefined}
                            <div><a onClick={() => this.resetFilters()}>Reset filters</a></div>
                            <div onClick={() => this.toggleExpandAll()}>
                                {this.getExpandAll() ? "▼ Hide" : "▶ Show"} all
                            </div>
                        </div>
                    </div>
                    <div className="search-categories">
                        {this.renderGrades()}
                        {this.renderSubjects()}
                        {this.renderClaims()}
                        {this.renderInteractionTypes()}
                    </div>
                </div>
            );
        }

        renderGrades() {
            const elementarySelected = (this.state.gradeLevels & GradeLevels.Elementary) == GradeLevels.Elementary;
            const middleSelected = (this.state.gradeLevels & GradeLevels.Middle) == GradeLevels.Middle;
            const highSelected = (this.state.gradeLevels & GradeLevels.High) == GradeLevels.High;

            const tags = [
                    <span className={(elementarySelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Elementary)}>

                        Grades 3-5
                    </span>,

                    <span className={(middleSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Middle)}>

                        Grades 6-8
                    </span>,

                    <span className={(highSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.High)}>

                        High School
                    </span>
            ];

            return (
                <div className="search-category" style={{ flexGrow: 3 }}>
                    <label onClick={() => this.toggleExpandGradeLevels()}>
                        {this.state.expandGradeLevels ? "▼" : "▶"} Grade Levels
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandGradeLevels ? tags : undefined}
                    </div>
                </div>
            );
        }

        renderSubject(subject: Subject) {
            const subjects = this.state.subjects || [];
            const className = (subjects.indexOf(subject.code) === -1 ? "" : "selected") + " tag";
            return (
                <span className={className}
                    onClick={() => this.toggleSubject(subject.code)}>

                    {subject.label}
                </span>
            );
        }

        renderSubjects() {
            const subjects = this.state.subjects || [];
            const tags = this.state.expandSubjects
                ? this.props.subjects.map(s => this.renderSubject(s))
                : undefined;

            return (
                <div className="search-category" style={{ flexGrow: 2 }}>
                    <label onClick={() => this.toggleExpandSubjects()}>
                        {this.state.expandSubjects ? "▼" : "▶"} Subjects
                    </label>
                    <div className="search-tags form-group">
                        {tags}
                    </div>
                </div>
            );
        }

        renderClaims() {
            const selectedClaims = this.state.claims || [];

            const makeClass = (claim: Claim) => (selectedClaims.indexOf(claim.code) === -1 ? "" : "selected") + " tag";
            const renderClaim = (claim: Claim) =>
                <span className={makeClass(claim)}
                    onClick={() => this.toggleClaim(claim.code)}>
                    {claim.label}
                </span>;

            // If no subjects are selected, use the entire list of subjects
            const selectedSubjectCodes = this.state.subjects || [];
            const subjects = selectedSubjectCodes.length !== 0
                ? this.props.subjects.filter(s => selectedSubjectCodes.indexOf(s.code) !== -1)
                : [];

            const tags = subjects.length === 0
                ? <span>Please select a subject.</span>
                : subjects
                    .reduce((cs: Claim[], s: Subject) => cs.concat(s.claims), [])
                    .map(renderClaim);

            return (
                <div className="search-category" style={{ flexGrow: this.props.subjects.length }}>
                    <label onClick={() => this.toggleExpandClaims()}>
                        {this.state.expandClaims ? "▼" : "▶"} Claims
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandClaims ? tags : undefined}
                    </div>
                </div>
            );
        }

        renderInteractionTypes() {
            const selectedInteractionTypes = this.state.interactionTypes || [];

            const makeClass = (it: InteractionType) => (selectedInteractionTypes.indexOf(it.code) === -1 ? "" : "selected") + " tag";
            const renderInteractionType = (it: InteractionType) =>
                <span className={makeClass(it)}
                    onClick={() => this.toggleInteractionType(it.code)}>
                    {it.label}
                </span>;
            
            const selectedSubjectCodes = this.state.subjects || [];
            const selectedSubjects = selectedSubjectCodes.length !== 0
                ? this.props.subjects.filter(subj => selectedSubjectCodes.indexOf(subj.code) !== -1)
                : [];

            const visibleInteractionTypes = selectedSubjects.length !== 0
                ? this.props.interactionTypes.filter(it => selectedSubjects.some(subj => subj.interactionTypeCodes.indexOf(it.code) !== -1))
                : [];

            const tags = visibleInteractionTypes.length === 0
                ? <span>Please select a subject.</span>
                : visibleInteractionTypes.map(renderInteractionType);

            return (
                <div className="search-category" style={{ flexGrow: this.props.interactionTypes.length }}>
                    <label onClick={() => this.toggleExpandInteractionTypes()}>
                        {this.state.expandInteractionTypes ? "▼" : "▶"} Interaction Types
                    </label>
                    <div className="search-tags form-group">
                        {this.state.expandInteractionTypes ? tags : undefined}
                    </div>
                </div>
            );
        }
    }
}
