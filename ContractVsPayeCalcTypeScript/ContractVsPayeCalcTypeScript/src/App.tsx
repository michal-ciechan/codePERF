import * as React from 'react';
import * as ReactDOM from 'react-dom';

interface IProps { }
interface IState { }

class SomeComponent extends React.Component<IProps, IState> {
    render() {
        return <div>Im so not working</div>;
    }

    get type() {
        return "test";
    }
}



var root = document.getElementById("root");

ReactDOM.render(<SomeComponent />, root);