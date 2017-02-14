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

namespace AboutThisItem {
    export interface Props {
        rubrics: Rubric[];
        itemCardViewModel: ItemCardViewModel
        // depthOfKnowledge: string; // TODO: Add when supported by xml
    }

    export class ATIComponent extends React.Component<Props, {}> {
        render() {
            const rubrics = this.props.rubrics.map((ru, i) => <RubricComponent {...ru} key={String(i)} />);
            return (
                <div className="modal fade" id="about-item-modal-container" tabIndex={-1} role="dialog" aria-labelledby="About Item Modal" aria-hidden="true">
                    <div className="modal-dialog about-item-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title">About This Item</h4>
                            </div>
                            <div className="modal-body">
                                <AboutThisItemDetailComponent {...this.props.itemCardViewModel} />
                                {rubrics}
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary" form="accessibility-form" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}

class RubricComponent extends React.Component<Rubric, {}> {
    render() {
        const label = `${this.props.language} Rubric`;

        const rubricEntries = this.props.rubricEntries.map((re, i) => <RubricEntryComponent {...re} key={String(i)} />);

        let rubricSamples: JSX.Element[] = [];
        let i: number = 0;
        for (const sample of this.props.samples) {
            const key = `${i}:`;
            const responses = sample.sampleResponses.map((sr, idx) => <SampleResponseComponent {...sr} key={key + String(idx)} />);
            rubricSamples.push(...responses);
            i++;
        }

        return (
            <Collapsible.CComponent label={label}>
                <h4>Rubrics</h4>
                {rubricEntries}
                <h4>Sample Responses</h4>
                {rubricSamples}
            </Collapsible.CComponent>
        );
    }
}

class RubricEntryComponent extends React.Component<RubricEntry, {}> {
    render() {
        const pointLabel = this.props.scorepoint == "1" ? "point" : "points";
        const label = `${this.props.name} (${this.props.scorepoint} ${pointLabel})`;
        return (
            <Collapsible.CComponent label={label}>
                <div dangerouslySetInnerHTML={{ __html: this.props.value }} />
            </Collapsible.CComponent>
        );
    }
}

class SampleResponseComponent extends React.Component<SampleResponse, {}> {
    render() {
        const label = `${this.props.name} (${this.props.scorePoint} Points)`;
        return (
            <Collapsible.CComponent label={label}>
                <div className="sample-response">
                    <b>Purpose: </b> {this.props.purpose} <br />
                    <b>Sample Response: </b> <br />
                    <div dangerouslySetInnerHTML={{ __html: this.props.sampleContent }} />
                </div>
            </Collapsible.CComponent>
        );
    }
}

class AboutThisItemDetailComponent extends React.Component<ItemCardViewModel, {}> {
    render() {
        const { bankKey, itemKey } = this.props;
        return (
            <div>
                <p className="card-text subject" tabIndex={0}>
                    <span className="card-text-label">Subject:</span>
                    <span className="card-text-value"> {this.props.subjectLabel}</span>
                </p>
                <p className="card-text grade" tabIndex={0}>
                    <span className="card-text-label">Grade:</span>
                    <span className="card-text-value"> {this.props.gradeLabel}</span>
                </p>
                <p className="card-text claim" tabIndex={0}>
                    <span className="card-text-label">Claim:</span>
                    <span className="card-text-value"> {this.props.claimLabel}</span>
                </p>
                <p className="card-text target" tabIndex={0}>
                    <span className="card-text-label">Target:</span>
                    <span className="card-text-value"> {this.props.target}</span>
                </p>
                <p className="card-text interaction-type" tabIndex={0}>
                    <span className="card-text-label">Item Type:</span>
                    <span className="card-text-value"> {this.props.interactionTypeLabel}</span>
                </p>
                <p className="card-text item-id" tabIndex={0}>
                    <span className="card-text-label">Item Id:</span>
                    <span className="card-text-value"> {this.props.itemKey}</span>
                </p>
        </div>
        );
    }
}

