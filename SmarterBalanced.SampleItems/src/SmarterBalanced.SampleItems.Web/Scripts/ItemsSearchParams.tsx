
namespace ItemSearchParams {

    export interface Props {
        subjectInteractionTypes: SubjectClaims;
        onChange: (params: SearchAPIParams) => void;
    }

    export interface State {
        terms?: string;
        gradeLevels?: GradeLevels;
        subjects?: string[];
        interactionTypes?: string[];
        timerID?: number;
    }

    export class Component extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = {
                terms: "",
                gradeLevels: GradeLevels.NA,
                subjects: [],
                interactionTypes: []
            };
        }

        /** This is a blatant race condition on component state, but will do for now. */
        beginChangeTimeout(): number {
            return setTimeout(() => {
                const params: SearchAPIParams = {
                    gradeLevels: this.state.gradeLevels || GradeLevels.All,
                    interactionTypes: this.state.interactionTypes || [],
                    subjects: this.state.subjects || [],
                    terms: this.state.terms || ""
                };
                this.props.onChange(params);
            }, 100);
        }

        toggleGrades(grades: GradeLevels) {
            if (this.state.timerID != null) {
                clearTimeout(this.state.timerID);
            }

            this.setState({
                // Exclusive OR to flip just the bits for the input grades
                gradeLevels: this.state.gradeLevels ^ grades,
                timerID: this.beginChangeTimeout()
            });

        }

        toggleSubject(subject: string) {
            if (this.state.timerID != null) {
                clearTimeout(this.state.timerID);
            }

            const subjects = this.state.subjects || [];
            const containsSubject = subjects.indexOf(subject) !== -1;
            this.setState({
                subjects: containsSubject ? subjects.filter(s => s !== subject) : subjects.concat([subject]),
                timerID: this.beginChangeTimeout()
            });
        }

        render() {
            return (
                <div className="search-params">
                    <div>
                        <p>Terms</p>
                        <input
                            value={this.state.terms}
                            onChange={e => this.setState({ terms: (e.target as HTMLInputElement).value })} />
                    </div>

                    <p>Grade Levels</p>
                    {this.renderGrades()}

                    <p>Subjects</p>
                    {this.renderSubjects()}

                    <p>Interaction Types</p>
                    {this.renderInteractionTypes()}

                </div>
            );
        }

        renderGrades() {
            const elementarySelected = (this.state.gradeLevels & GradeLevels.Elementary) == GradeLevels.Elementary;
            const middleSelected = (this.state.gradeLevels & GradeLevels.Middle) == GradeLevels.Middle;
            const highSelected = (this.state.gradeLevels & GradeLevels.High) == GradeLevels.High;

            return (
                <div className="search-tags">
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
                <div className="search-tags">
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
            const selectedSubjects = this.state.subjects || [];
            if (selectedSubjects.length === 0) {
                return <div><p className="placeholder-text">Select a subject to show interaction types</p></div>;
            }

            const selectedInteractionTypes = this.state.interactionTypes || [];

            let elements: JSX.Element[] = [];
            for (const subject of selectedSubjects) {
                for (const interactionType of this.props.subjectInteractionTypes[subject]) {
                    const element =
                        <span className={(selectedInteractionTypes.indexOf(interactionType.value) === -1 ? "" : "selected") + " tag"}>
                            {interactionType.text}
                        </span>;
                    elements.push(element);
                }
            }

            return (
                <div className="search-tags">
                    {elements}
                </div>
            );
        }
    }
}
