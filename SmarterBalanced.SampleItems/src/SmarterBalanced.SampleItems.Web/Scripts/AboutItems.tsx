
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

    export class AIComponent extends React.Component<AboutThisItemViewModel, {}>{
        constructor(props: AboutThisItemViewModel) {
            super(props);
        }

        render() {
            return (
                <div>
                    TODO:
                    - Interaction types select list
                    - iframe with URL
                </div>
            );
        }
    }

}


function initialzeAboutItems(viewModel: AboutThisItemViewModel){
    ReactDOM.render(
        <AboutItems.AIComponent {...viewModel} />,
        document.getElementById("about-items") as HTMLElement
    );
}