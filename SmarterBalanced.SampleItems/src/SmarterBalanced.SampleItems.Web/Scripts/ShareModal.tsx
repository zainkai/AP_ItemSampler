namespace Share {
    function getItemUrl(): string {
        let fullUrl = window.location.href;
        let url = fullUrl.split("&isaap=")[0];
        return url;
    }

    interface Props {
        iSAAP: string;
    }

    export class ShareModal extends React.Component<Props, {}>{
        constructor(props: Props) {
            super(props);
        }

        render() {
            let url = getItemUrl() + "&isaap=" + this.props.iSAAP
            return (
                <div className="modal fade" id="share-modal-container" tabIndex={-1} role="dialog" aria-hidden="true">
                    <div className="modal-dialog share-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title" id="myModalLabel">Share</h4>
                            </div>
                            <div className="modal-body">
                                <span>The following URL can be used to load this question with your currently saved accessibility options.</span>
                                <div className="url-display">
                                    <a href={url}>{url}</a>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}