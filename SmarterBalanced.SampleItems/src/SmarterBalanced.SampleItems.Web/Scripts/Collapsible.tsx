namespace Collapsible {

    interface Props {
        label: string;
        body?: JSX.Element;
        areAllCollapsed: boolean;
        childNodes?: JSX.Element[];
        className: string;
        style?: React.CSSProperties;
    }

    interface State {
        isCollapsed: boolean;
        childNodes?: JSX.Element[];
    }

    export class NodeComponent extends React.Component<Props, State> {
        constructor(props: Props) {
            super(props);
            this.state = {
                isCollapsed: props.areAllCollapsed,
                childNodes: props.childNodes
            };
        }

        componentWillReceiveProps(newProps: Props) {
            this.toggleAll(newProps.areAllCollapsed);
        }

        toggleState(isCollapsed: boolean, childNodes?: JSX.Element[]) {
            this.setState({
                isCollapsed: isCollapsed,
                childNodes: childNodes
            });
        }

        toggleAll(isCollapsed: boolean) {
            let newChildNodes: any = undefined;
            if (this.state.childNodes != null) {
                newChildNodes = []
                for (let i = 0; i < this.state.childNodes.length; i++) {
                    const child = this.state.childNodes[i];
                    let props = Object.apply(child.props);
                    props.areAllCollapsed = isCollapsed;
                    newChildNodes.push(React.cloneElement(child, props));
                }
            }
            this.toggleState(isCollapsed, newChildNodes);
        }

        toggleCollapse() {
            this.toggleState(!this.state.isCollapsed, this.state.childNodes);
        }

        render() {
            let label = "";
            let style = {};
            if (this.state.isCollapsed) {
                label = "▶ " + this.props.label;
                style = { display: "None" }
            }
            else {
                label = "▼ " + this.props.label;
            }

            return (
                <div className={this.props.className} style={this.props.style} aria-expanded={!this.state.isCollapsed}>
                    <a role="button" className="collapsible-label link-button" href="#" onClick={() => this.toggleCollapse()} tabIndex={0}>
                        {label}
                    </a>
                    <div className="collapsible-body" style={style} aria-hidden={this.state.isCollapsed}>
                        {this.props.body}
                        <ul className="collapsible-children">
                            {this.state.childNodes}
                        </ul>
                    </div>
                </div>
            );
        }
    }

}
