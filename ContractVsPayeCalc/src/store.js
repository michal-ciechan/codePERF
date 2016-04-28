import { createStore } from 'redux'
import Immutable from 'immutable';
import {calcAppReducer} from './reducers'

import { FIELDS, ForceRecalc } from './actions'

// Initial Field Values
var initialFieldValues = Immutable.Map({})
  .set(FIELDS.SALARY, 75000)
  .set(FIELDS.BONUS, 15)
  .set(FIELDS.BANK_HOLIDAYS, 12)
  .set(FIELDS.HOLIDAYS, 25)
  .set(FIELDS.SICK_DAYS, 3)
  .set(FIELDS.PENSION, 12.5)
  
const initialState = Immutable.Map({inputs: initialFieldValues});

const Store = createStore(calcAppReducer, initialState)

// Initial Calc values
Store.dispatch(ForceRecalc());

export default Store