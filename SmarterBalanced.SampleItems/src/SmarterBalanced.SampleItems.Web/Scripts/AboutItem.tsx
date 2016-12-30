interface SampleResponse {
    purpose: string;
    scorePoint: string;
    name: string;
    sampleContent: string;
}

interface RubricSample {
    maxValue: string;
    minValue: string;
    sampleResponses: SampleResponse[];
}

interface RubricEntry {
    scorepoint: string;
    name: string;
    value: string;
}

interface Rubric {
    language: string;
    rubricEntries: RubricEntry[];
    samples: RubricSample[];
}

interface AboutItemViewModel {
    itemKey: number;
    commonCoreStandardsId: string;
    targetId: string;
    grade: GradeLevels;
    rubrics: Rubric[];
    // depthOfKnowledge: string; // TODO: Add when supported by xml
}


namespace AboutItem {

    interface Props {
        children: JSX.Element[];
    }

    interface State {
        isCollapsed: boolean;
        childNodes: JSX.Element[];
    }

    export class RootComponent extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = {
                isCollapsed: true,
                childNodes: props.children
            };
        }

        toggleAll() {
            const newState = !this.state.isCollapsed;
            let newChildren: JSX.Element[] = []
            for (let i = 0; i < this.state.childNodes.length; i++) {
                const child = this.state.childNodes[i];
                let props = Object.apply(child.props);
                props.areAllCollapsed = newState;
                newChildren.push(React.cloneElement(child, props));
            }

            this.setState({
                isCollapsed: newState,
                childNodes: newChildren
            });
        }

        render() {
            const label = this.state.isCollapsed ? "▶ Show All" : "▼ Hide All";
            return (
                <div className="about-item-container">
                    <a role="button" id="toggle-all-about-item" className="link-button" href="#" onClick={() => this.toggleAll()}>
                        {label}
                    </a>
                    {this.state.childNodes}
                </div>
            );
        }

    }

}

class InitializeAboutItem {
    rootChildren: JSX.Element[];
    viewModel: AboutItemViewModel;
    isCollapsed: boolean = true;
    rootClassName: string;
    elementClassName: string = "about-item-data";

    constructor(viewModel: AboutItemViewModel, rootClassName: string) {
        this.viewModel = viewModel;
        this.rootClassName = rootClassName;
        this.rootChildren = [];

        this.pushTextElems();
        this.pushRubrics();
    }

    renderElements() {
        ReactDOM.render(
            <AboutItem.RootComponent children={this.rootChildren} aria-expanded={!this.isCollapsed}/>,
            document.getElementById(this.rootClassName) as HTMLElement
        );
    }

    pushTextComponent(key: string, label: string, bodyText: string) {
        if (label && bodyText) {
            this.rootChildren.push(this.buildTextComponent(key, label, bodyText));
        }
    }

    buildTextComponent(key: string, label: string, bodyText: string) {
        const htmlBody = (<div>{bodyText}</div>);
        return this.buildGenericComponent(key, label, htmlBody, undefined, undefined);
    }

    // Helper to build a generic Collapsible.NodeComponent
    buildGenericComponent(
        key: string,
        label: string,
        body?: JSX.Element,
        style?: React.CSSProperties,
        childNodes?: JSX.Element[]) {
        return (
            <Collapsible.NodeComponent
                key={key}
                areAllCollapsed={this.isCollapsed}
                className={this.elementClassName}
                label={label}
                body={body}
                style={style}
                childNodes={childNodes}
                />
        );
    }

    pushTextElems() {
        this.pushTextComponent("item-id", "Item Id", this.viewModel.itemKey.toString());
        this.pushTextComponent("ccss", "Common Core State Standards", this.viewModel.commonCoreStandardsId);
        this.pushTextComponent("target", "Target Id", this.viewModel.targetId);

        const grade = GradeLevels.toString(this.viewModel.grade);
        this.pushTextComponent("target-grade", "Target Grade", grade);
    }

    buildRubricEntry(rubricEntry: RubricEntry, key: string) {
        const pointLabel = rubricEntry.scorepoint == "1" ? "point" : "points";
        const label = `${rubricEntry.name} (${rubricEntry.scorepoint} ${pointLabel})`;
        const body = (
            <div>
                <div dangerouslySetInnerHTML={{ __html: rubricEntry.value }} />
            </div>
        );

        return this.buildGenericComponent(key, label, body, undefined, undefined);
    }

    buildRubricEntries(rubricEntries: RubricEntry[], parentKey: string) {
        let rubricsEntryElems: JSX.Element[] = [];
        if (rubricEntries == null) {
            return rubricsEntryElems;
        }

        for (let i = 0; i < rubricEntries.length; i++) {
            const rubricEntry: RubricEntry = rubricEntries[i];
            const key = `${parentKey}-${i}`;
            rubricsEntryElems.push(this.buildRubricEntry(rubricEntry, key));
        }

        return rubricsEntryElems;
    }

    buildSampleResponse(sampleResponse: SampleResponse, key: string) {
        const label = `${sampleResponse.name} (${sampleResponse.scorePoint} Points)`;
        const body = (
            <div className="sample-response">
                <b>{"Purpose: "}</b> {sampleResponse.purpose} <br />
                <b>{"Sample Response: "}</b><br />
                <div dangerouslySetInnerHTML={{ __html: sampleResponse.sampleContent }} />
            </div>
        );

        return this.buildGenericComponent(key, label, body, undefined, undefined);
    }

    buildSampleResponses(sampleResponses: SampleResponse[], parentKey: string) {
        let sampleResponseElems: JSX.Element[] = [];
        if (sampleResponses == null) {
            return sampleResponseElems;
        }

        for (let i = 0; i < sampleResponses.length; i++) {
            const sampleResponse = sampleResponses[i];
            const key = `${parentKey}-${i}`;
            sampleResponseElems.push(this.buildSampleResponse(sampleResponse, key));
        }

        return sampleResponseElems;
    }

    buildRubricSample(rubricSample: RubricSample, parentKey: string) {
        const label = `Sample Response (Minimum Score: ${rubricSample.minValue}, Maximum Score ${rubricSample.maxValue})`;
        const childNodes = this.buildSampleResponses(rubricSample.sampleResponses, parentKey);

        return this.buildGenericComponent(parentKey, label, undefined, undefined, childNodes);
    }

    buildRubricSamples(rubricSamples: RubricSample[], parentKey: string) {
        let rubricsEntryElems: JSX.Element[] = [];
        if (rubricSamples == null) {
            return rubricsEntryElems;
        }

        for (let i = 0; i < rubricSamples.length; i++) {
            const rubricSample = rubricSamples[i];
            const key = `${parentKey}-${i}`;
            rubricsEntryElems.push(this.buildRubricSample(rubricSample, key));
        }

        return rubricsEntryElems;
    }

    buildRubric(rubric: Rubric, idx: string) {
        const label = `${rubric.language} Rubric`;
        const key = `${rubric.language}-rubric-${idx}`;

        const rubricEntries = this.buildRubricEntries(rubric.rubricEntries, `${key}-entries`);
        const rubricSamples = this.buildRubricSamples(rubric.samples, `${key}-samples`);
        
        return this.buildGenericComponent(key, label, undefined, undefined, rubricEntries.concat(rubricSamples));
    }

    pushRubrics() {
        if (this.viewModel.rubrics == null) {
            return;
        }

        const rubrics = this.viewModel.rubrics;
        for (let i = 0; i < rubrics.length; i++) {
            const rubric = rubrics[i];
            this.rootChildren.push(this.buildRubric(rubric, i.toString()));
        }
    }

}

function initializeAboutItem(viewModel: AboutItemViewModel) {
    new InitializeAboutItem(viewModel, "about-item-container").renderElements();
}