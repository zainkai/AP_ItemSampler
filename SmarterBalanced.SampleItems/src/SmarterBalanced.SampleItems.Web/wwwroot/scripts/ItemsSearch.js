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
function itemPageLink(bankKey, itemKey) {
    window.location.href = "/Item/Details?bankKey=" + bankKey + "&itemKey=" + itemKey;
}
var ItemCard = (function (_super) {
    __extends(ItemCard, _super);
    function ItemCard() {
        _super.apply(this, arguments);
    }
    ItemCard.prototype.render = function () {
        var _a = this.props, bankKey = _a.bankKey, itemKey = _a.itemKey;
        return (React.createElement("div", {className: "card card-block", onClick: function (e) { return itemPageLink(bankKey, itemKey); }}, 
            React.createElement("div", {className: "card-contents"}, 
                React.createElement("h4", {className: "card-title"}, 
                    bankKey, 
                    "-", 
                    itemKey), 
                React.createElement("p", {className: "card-text"}, 
                    "Claim: ", 
                    this.props.claim), 
                React.createElement("p", {className: "card-text"}, 
                    "Grade: ", 
                    this.props.grade), 
                React.createElement("p", {className: "card-text"}, 
                    "Subject: ", 
                    this.props.subject), 
                React.createElement("p", {className: "card-text"}, 
                    "Interaction Type: ", 
                    this.props.interactionType))
        ));
    };
    return ItemCard;
}(React.Component));
var data = { terms: "foo", gradeLevels: GradeLevels.Elementary, subjects: ["ELA"], claimType: "" };
$.ajax({
    dataType: "json",
    url: "/ItemsSearch/search",
    traditional: true,
    data: data,
    success: onSearch
});
function onSearch(data) {
    var itemCards = data.map(function (digest) { return React.createElement(ItemCard, __assign({}, digest)); });
    ReactDOM.render(React.createElement("div", {className: "container"}, itemCards), document.getElementById("container"));
}
//# sourceMappingURL=ItemsSearch.js.map