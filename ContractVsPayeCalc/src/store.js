import { createStore } from "redux"
import Immutable from "immutable";
import {calcAppReducer} from "./reducers"

import { FIELDS, ForceRecalc } from "./actions"

// Initial Field Values
var initialFieldValues = Immutable.Map({})
  .set(FIELDS.SALARY, 65000)
  .set(FIELDS.BONUS, 15)
  .set(FIELDS.BANK_HOLIDAYS, 12)
  .set(FIELDS.HOLIDAYS, 25)
  .set(FIELDS.SICK_DAYS, 3)
  .set(FIELDS.PENSION, 12.5)
  .set(FIELDS.VAT_FLAT_RATE, 14.5)
  .set(FIELDS.VAT_EXPENSES, 4000)
  .set(FIELDS.TRAVEL_EXPENSES, 2400)
  .set(FIELDS.NETWORKING_EXPENSES, 600)
  .set(FIELDS.OFFICE_EXPENSES, 1200)
  .set(FIELDS.OTHER_EXPENSES, 5000)
  .set(FIELDS.DIRECTORS_PAY, 8000)

const initialState = Immutable.Map({inputs: initialFieldValues});

const Store = createStore(calcAppReducer, initialState)

// Initial Calc values
Store.dispatch(ForceRecalc());

export default Store
