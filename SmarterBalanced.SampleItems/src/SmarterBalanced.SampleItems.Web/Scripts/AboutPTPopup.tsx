namespace AboutPTPopup {
    export interface Props {
        subject: string;
        description: string;
    }

    function readCookie(name: string): string | undefined {
        var cookie = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
        return cookie ? cookie.pop() : '';
    }

    export class Modal extends React.Component<Props, {}> {

        shouldShowOnLoad(): void {
            var visitedBefore = false;
            //Cookies only store strings
            if (this.props.subject.toLowerCase() === "math") {
                visitedBefore = (readCookie("visitedMathPerfItem") == "true");
                document.cookie = "visitedMathPerfItem=true";
            } else if (this.props.subject.toLowerCase() === "ela") {
                visitedBefore = (readCookie("visitedELAPerfItem") == "true");
                document.cookie = "visitedELAPerfItem=true";
            }
            if (!visitedBefore) {
                $(window).load(function () {
                    $('#about-performance-tasks-popup-modal-container').modal('show');
                });
            }
        }
        getSubjectText(): string {
            switch (this.props.subject.toLowerCase()) {
                case "math":
                    return "Math";
                case "ela":
                    return "ELA";
                default:
                    return "";
            }
        }

        render() {
            this.shouldShowOnLoad();
            const subject = this.getSubjectText();
            return (
                <div className="modal fade" id="about-performance-tasks-popup-modal-container" tabIndex={-1} role="dialog" aria-hidden="true">
                    <div className="modal-dialog share-modal" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 className="modal-title" id="myModalLabel">About {subject} Performance Tasks</h4>
                            </div>
                            <div className="modal-body">
                                <p dangerouslySetInnerHTML={{ __html: this.props.description }} />
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