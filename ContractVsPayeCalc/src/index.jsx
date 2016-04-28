import React from 'react';
import ReactDOM from 'react-dom';
import { render } from 'react-dom'
import App from './App.jsx';
import {Store} from './App.jsx';
require("./App.jsx")

ReactDOM.render(
    <App store={Store} />,
document.getElementById('root')
);
