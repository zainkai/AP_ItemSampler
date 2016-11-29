
namespace ItemSearchParams {

    export interface Props {
        interactionTypes: InteractionType[];
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
                    interactionTypes: this.state.interactionTypes || [],
                    subjects: this.state.subjects || []
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
            const subjects = this.state.subjects || [];
            const containsSubject = subjects.indexOf(subject) !== -1;
            this.setState({
                subjects: containsSubject ? subjects.filter(s => s !== subject) : subjects.concat([subject])
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
                                {this.getExpandAll() ? "▼" : "▶"} Show all
                            </div>
                        </div>
                    </div>
                    <div className="search-categories">
                        {this.renderGrades()}
                        {this.renderSubjects()}
                        {this.renderInteractionTypes()}
                    </div>
                </div>
            );
        }

        renderGrades() {
            const elementarySelected = (this.state.gradeLevels & GradeLevels.Elementary) == GradeLevels.Elementary;
            const middleSelected = (this.state.gradeLevels & GradeLevels.Middle) == GradeLevels.Middle;
            const highSelected = (this.state.gradeLevels & GradeLevels.High) == GradeLevels.High;

            const tags = (
                <div className="search-tags form-group" style={{ flexGrow: 3 }}>
                    <span className={(elementarySelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Elementary)}>

                        Grades 3-5
                    </span>

                    <span className={(middleSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Middle)}>

                        Grades 6-8
                    </span>

                    <span className={(highSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.High)}>

                        High School
                    </span>
                </div>
            );

            return (
                <div className="search-category">
                    <label onClick={() => this.toggleExpandGradeLevels()}>
                        {this.state.expandGradeLevels ? "▼" : "▶"} Grade Levels
                    </label>
                    {this.state.expandGradeLevels ? tags : undefined}
                </div>
            );
        }

        renderSubjects() {
            const subjects = this.state.subjects || [];
            const tags = (
                <div className="search-tags form-group" style={{ flexGrow: 2 }}>
                    <span className={(subjects.indexOf("ELA") === -1 ? "" : "selected") + " tag"}
                        onClick={() => this.toggleSubject("ELA")}>

                        English Language Arts
                    </span>
                    <span className={(subjects.indexOf("MATH") === -1 ? "" : "selected") + " tag"}
                        onClick={() => this.toggleSubject("MATH")}>

                        Math
                    </span>
                </div>
            );
            return (
                <div className="search-category">
                    <label onClick={() => this.toggleExpandSubjects()}>
                        {this.state.expandSubjects ? "▼" : "▶"} Subjects
                    </label>
                    {this.state.expandSubjects ? tags : undefined}
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
            const tags = this.props.interactionTypes.map(renderInteractionType);

            return (
                <div className="search-category" style={{ flexGrow: tags.length }}>
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
