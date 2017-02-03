
interface InteractionType {
    code: string;
    label: string;
    description: string;
    order?: number;
}

interface AboutThisItemViewModel {
    interactionTypes: InteractionType[];
    itemUrl: string;
}

namespace AboutItems {

    interface state {
        selectedCode: string;
        itemUrl: string;
    }

    export class AIComponent extends React.Component<AboutThisItemViewModel, state>{
        constructor(props: AboutThisItemViewModel) {
            super(props);

            const code = this.props.interactionTypes.length ? this.props.interactionTypes[0].code : "";
            this.state = {
                selectedCode: code,
                itemUrl: this.props.itemUrl
            };

            this.handleChange = this.handleChange.bind(this);
            this.setNewUrl = this.setNewUrl.bind(this);
        }

        handleChange(e: any) {
            const newCode = e.currentTarget.value
            if (newCode === this.state.selectedCode) {
                return;
            }

            this.getNewUrl(newCode);

            this.setState(Object.assign({}, this.state, { selectedCode: newCode }) as state)
        }

        getNewUrl(newCode: string) {
            const params = {
                interactionTypeCode: newCode
            };

            $.ajax({
                dataType: "JSON",
                type: "GET",
                url: "/AboutItems/GetItemUrl",
                data: params,
                success: this.setNewUrl
            });
        }

        setNewUrl(newUrl: string) {
            // TODO: Handle if url was null (no item found)
            this.setState(Object.assign({}, this.state, { itemUrl: newUrl }) as state)
        }

        renderAboutTestItemsText() {
            let aboutTestItemsText = "Smarter Balanced assessments use a variety of item types to accurately measure what students know and can do. To learn more and see an example, select an item type below.";
            return (<div>{aboutTestItemsText}</div>);
        }

        renderDescription() {
            let desc = "";
            for (let it of this.props.interactionTypes) {
                if (it.code === this.state.selectedCode) {
                    desc = it.description;
                }
            }
            return (<div>{desc}</div>);
        }

        renderInteractionTypesSelect() {
            let items = [];
            for (let i of this.props.interactionTypes) {
                items.push(
                    <option key={i.code} value={i.code}> {i.label} </option>
                );
            }

            return (
                <select className="form-control" onChange={this.handleChange}>
                    {items}
                </select>
            );
        }

        render() {
            return (
                <div className="abt-items-parent">
                    <div className="abt-test-items-info">
                        <div className="abt-test-items-text">
                            {this.renderAboutTestItemsText()}
                        </div>
                        <div className="abt-items-iteraction-dropdown">
                            <div className="form-group">
                            {this.renderInteractionTypesSelect() }
                            </div>
                        </div>
                        <div className="abt-items-desc">
                            {this.renderDescription()}
                        </div>
                    </div>
                    <div className="abt-item-frame">
                        <ItemFrame url={this.state.itemUrl} />
                    </div>
                </div>
            );
        }
    }

}


function initializeAboutItems(viewModel: AboutThisItemViewModel) {
    ReactDOM.render(
        <AboutItems.AIComponent {...viewModel} />,
        document.getElementById("about-items") as HTMLElement
    );
}