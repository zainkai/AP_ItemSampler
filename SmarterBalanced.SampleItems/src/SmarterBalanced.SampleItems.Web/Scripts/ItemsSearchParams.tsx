
namespace ItemSearchParams {

    export interface Props {
        interactionTypes: InteractionType[];
        onChange: (params: SearchAPIParams) => void;
    }

    export interface State {
        terms?: string;
        gradeLevels?: GradeLevels;
        subjects?: string[];
        interactionTypes?: string[];
    }

    export class Component extends React.Component<Props, State> {
        // TODO: since the callback property exists on setState, should this be in the state interface instead of the component class?
        timeoutToken?: number;

        constructor(props: Props) {
            super(props);
            this.state = {
                terms: "",
                gradeLevels: GradeLevels.NA,
                subjects: [],
                interactionTypes: []
            };
        }

        beginChangeTimeout() {
            if (this.timeoutToken !== undefined) {
                clearTimeout(this.timeoutToken);
            }

            this.timeoutToken = setTimeout(() => {
                const params: SearchAPIParams = {
                    gradeLevels: this.state.gradeLevels || GradeLevels.All,
                    interactionTypes: this.state.interactionTypes || [],
                    subjects: this.state.subjects || [],
                    terms: this.state.terms || ""
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

        render() {
            return (
                <form className="search-params">
                    <div className="form-group">
                        <input
                            name="searchTerms"
                            className="form-control"
                            value={this.state.terms}
                            placeholder="Search"
                            onChange={e => this.setState({ terms: (e.target as HTMLInputElement).value })} />
                    </div>

                    <h4>Filter Tags</h4>
                    <label>Grade Levels</label>
                    {this.renderGrades()}

                    <label>Subjects</label>
                    {this.renderSubjects()}

                    <label>Interaction Types</label>
                    {this.renderInteractionTypes()}
                </form>
            );
        }

        renderGrades() {
            const elementarySelected = (this.state.gradeLevels & GradeLevels.Elementary) == GradeLevels.Elementary;
            const middleSelected = (this.state.gradeLevels & GradeLevels.Middle) == GradeLevels.Middle;
            const highSelected = (this.state.gradeLevels & GradeLevels.High) == GradeLevels.High;

            return (
                <div className="search-tags form-group">
                    <span className={(elementarySelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Elementary)}>

                        Elementary School
                    </span>

                    <span className={(middleSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.Middle)}>

                        Middle School
                    </span>

                    <span className={(highSelected ? "selected" : "") + " tag"}
                        onClick={() => this.toggleGrades(GradeLevels.High)}>

                        High School
                    </span>
                </div>
            );
        }

        renderSubjects() {
            const subjects = this.state.subjects || [];
            return (
                <div className="search-tags form-group">
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
        }

        renderInteractionTypes() {
            const selectedInteractionTypes = this.state.interactionTypes || [];

            const makeClass = (it: InteractionType) => (selectedInteractionTypes.indexOf(it.code) === -1 ? "" : "selected") + " tag";
            const renderInteractionType = (it: InteractionType) =>
                <span className={makeClass(it)}
                    onClick={() => this.toggleInteractionType(it.code)}>
                    {it.label}
                </span>;

            return (
                <div className="search-tags form-group">
                    {this.props.interactionTypes.map(renderInteractionType)}
                </div>
            );
        }
    }
}
