interface FrameProps {
    url: string;
}

class ItemFrame extends React.Component<FrameProps, {}> {
    constructor(props: FrameProps) {
        super(props);
    }

    render() {
        return (
            <div className="itemViewerFrame" tabIndex={0}>
                <iframe id="itemviewer-iframe" className="itemviewer-iframe"
                    src={this.props.url}></iframe>
            </div>
        );
    }
}
