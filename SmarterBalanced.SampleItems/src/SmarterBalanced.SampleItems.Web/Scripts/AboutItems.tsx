
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

        renderDescription() {
            let desc = "";
            for (let it of this.props.interactionTypes) {
                if (it.code === this.state.selectedCode) {
                    desc = it.description;
                }
            }
            return (
                <div dangerouslySetInnerHTML={{ __html: desc }} className= "aboutitems-desc" />
            );
        }

        renderInteractionTypesSelect() {
            let items: JSX.Element[] = [];
            for (let i of this.props.interactionTypes) {
                items.push(
                    <option key={i.code} value={i.code}> {i.label} </option>
                );
            }

            return (
                <select className="aboutitems-dropdown" onChange={this.handleChange}>
                    {items}
                </select>
            );
        }

        render() {
            return (
                <div className="aboutitems-parents">

                    <div className="aboutitems-info">
                        <div className="aboutitems-text">
                            Smarter Balanced assessments use a variety of item
                             types to accurately measure what students know and can do.
                             To learn more and see an example, select an item type below.
                        </div>
                        {this.renderInteractionTypesSelect()}
                        {this.renderDescription()}
                    </div>

                    <div className="aboutitem-iframe">
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