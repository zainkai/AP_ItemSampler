// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var GlobalAccommodations;
(function (GlobalAccommodations) {
    var Component = (function (_super) {
        __extends(Component, _super);
        function Component() {
            _super.apply(this, arguments);
            this.state = this.props.initialState;
        }
        Component.prototype.render = function () {
            var _a = this.state, presentation = _a.presentation, textToSpeech = _a.textToSpeech, masking = _a.masking, colorContrast = _a.colorContrast, highlighter = _a.highlighter, studentComments = _a.studentComments;
            return (React.createElement("div", null, 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Presentation:"), 
                    comboBox(presentation)), 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Text-To-Speech:"), 
                    comboBox(textToSpeech)), 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Masking:"), 
                    comboBox(masking)), 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Color Contrast:"), 
                    comboBox(colorContrast)), 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Highlighter:"), 
                    React.createElement("input", {type: "checkbox", selected: highlighter})), 
                React.createElement("div", null, 
                    React.createElement("p", {style: { display: "inline-block" }}, "Student Comments:"), 
                    comboBox(studentComments))));
        };
        return Component;
    }(React.Component));
    GlobalAccommodations.Component = Component;
    function comboBox(accommodation) {
        var options = accommodation.options.map(function (name, idx) {
            var isSelected = idx === accommodation.selectedIndex;
            return React.createElement("option", {selected: isSelected}, name);
        });
        return (React.createElement("select", null, options));
    }
})(GlobalAccommodations || (GlobalAccommodations = {}));
//# sourceMappingURL=GlobalAccommodationsComponent.js.map