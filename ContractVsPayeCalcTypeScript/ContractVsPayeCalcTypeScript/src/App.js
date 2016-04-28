"use strict";
const React = require('react');
const ReactDOM = require('react-dom');
class SomeComponent extends React.Component {
    render() {
        return React.createElement("div", null, "Im so not working");
    }
    get type() {
        return "test";
    }
}
var root = document.getElementById("root");
ReactDOM.render(React.createElement(SomeComponent, null), root);
