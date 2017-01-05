interface FrameProps {
    baseUrl: string;
    accessibilityString: string;
    url: string;
}

class ItemFrame extends React.Component<FrameProps, {}> {
    constructor(props: FrameProps) {
        super(props);
    }

    render() {
        return (
            <div className="itemViewerFrame">
                <iframe id="itemviewer-iframe" className="itemviewer-iframe"
                    src={this.props.url}></iframe>
            </div>
        );
    }
}
