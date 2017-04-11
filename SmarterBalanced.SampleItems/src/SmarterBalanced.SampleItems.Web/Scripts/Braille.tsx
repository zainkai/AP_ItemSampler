namespace Braille {

    export interface Props {
        currentSelectionCode: string;
        brailleItemCodes: string[];
        braillePassageCodes: string[];
        bankKey: number;
        itemKey: number;
    }

    export class BrailleLink extends React.Component<Props, {}> {
        constructor(props: Props) {
            super(props);
        }

        buildUrl(bankKey: number, itemKey: number, ): string {
            var brailleType = "";
            if (typeof (this.props.brailleItemCodes) != 'undefined' && this.props.brailleItemCodes.indexOf(this.props.currentSelectionCode) > -1) {
                var brailleLoc = this.props.brailleItemCodes.indexOf(this.props.currentSelectionCode);
                brailleType = this.props.brailleItemCodes[brailleLoc];
                return "/Item/Braille?bankKey=" + bankKey + "&itemKey=" + itemKey + "&brailleCode=" + brailleType;
                
            }
            return "";
        }

        render() {
            let brailleUrl = this.buildUrl(this.props.bankKey, this.props.itemKey);
            if (brailleUrl == "") {
                return null;
            } else { 
                return (
                    <a className="item-nav-btn" href={brailleUrl} download >
                            <span className="glyphicon glyphicon-download-alt glyphicon-pad"/>
                            Download Braille Embossing
                        </a>
                  );
            }
        }
    }
}