import React from 'react'
import { connect } from 'react-redux'
import { Provider } from 'react-redux'

import Store from './Store';
import PayeFields from './inputs/fields.jsx'

const App2 = ({ state }) => (
  <Provider store={Store}>
    <PayeFields />
  </Provider>
)

export { Store };
export default connect()(App2);