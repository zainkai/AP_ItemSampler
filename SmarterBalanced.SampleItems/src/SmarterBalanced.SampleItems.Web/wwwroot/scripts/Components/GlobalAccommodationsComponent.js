// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
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
            var _this = this;
            return (React.createElement("div", {className: "accommodations"}, 
                React.createElement("h1", null, "All Accommodations"), 
                this.renderFields(), 
                React.createElement("button", {onClick: function () { return _this.report(); }}, "Submit")));
        };
        Component.prototype.renderFields = function () {
            var _a = this.state, presentation = _a.presentation, textToSpeech = _a.textToSpeech, masking = _a.masking, colorContrast = _a.colorContrast, highlighter = _a.highlighter, studentComments = _a.studentComments;
            return (React.createElement("div", null, 
                React.createElement(ComboBox.Component, __assign({}, presentation)), 
                React.createElement(ComboBox.Component, __assign({}, textToSpeech)), 
                React.createElement(ComboBox.Component, __assign({}, masking)), 
                React.createElement(ComboBox.Component, __assign({}, colorContrast)), 
                React.createElement("div", null, 
                    React.createElement("label", {htmlFor: "select-highlighter"}, "Highlighter:"), 
                    React.createElement("input", {type: "checkbox", id: "select-highlighter", selected: highlighter})), 
                React.createElement(ComboBox.Component, __assign({}, studentComments))));
        };
        Component.prototype.report = function () {
            alert(JSON.stringify(this.state));
        };
        return Component;
    }(React.Component));
    GlobalAccommodations.Component = Component;
})(GlobalAccommodations || (GlobalAccommodations = {}));
var ComboBox;
(function (ComboBox) {
    var Component = (function (_super) {
        __extends(Component, _super);
        function Component() {
            _super.apply(this, arguments);
            this.state = {
                value: this.props.initialValue || this.props.options[0]
            };
        }
        Component.prototype.render = function () {
            var _this = this;
            var options = this.props.options.map(function (name) { return React.createElement("option", {key: _this.props.id + "-" + name}, name); });
            return (
            // should event handlers be passed down?
            // e => this.props.onChange((e.target as HTMLInputElement).value)
            React.createElement("div", null, 
                React.createElement("label", {htmlFor: this.props.id}, this.props.name), 
                React.createElement("select", {id: this.props.id, onChange: function (e) { return _this.onChange(e); }}, options)));
        };
        Component.prototype.onChange = function (e) {
            this.setState({
                value: e.target.value
            });
        };
        return Component;
    }(React.Component));
    ComboBox.Component = Component;
})(ComboBox || (ComboBox = {}));
//# sourceMappingURL=GlobalAccommodationsComponent.js.map