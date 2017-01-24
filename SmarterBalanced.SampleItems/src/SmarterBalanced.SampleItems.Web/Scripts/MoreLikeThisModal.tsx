namespace MoreLikeThis {

    interface Props {
        item: ItemCardViewModel;
        moreLikeThisVM: ItemPage.MoreLikeThisViewModel;
    }

    export class Modal extends React.Component<Props, {}> {
        render() {
            return (
                <div className="modal fade" id="more-like-this-modal-container" tabIndex={-1} role="dialog" aria-labelledby="More Like This Modal" aria-hidden="true">
                    <div className="modal-dialog more-like-this-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title" id="myModalLabel">More Like This</h4>
                            </div>
                            <div className="modal-body">
                                Items similar to {this.props.item.gradeLabel} item {`${this.props.item.bankKey}-${this.props.item.itemKey}`}:
                                <br />
                                TODO: Content
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-primary btn-cancel" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                );
        }

    }
    
}