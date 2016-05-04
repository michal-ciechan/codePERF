import { combineReducers } from "redux-immutable"
//import { combineReducers } from 'redux'
import Immutable from "immutable";

import { FIELDS, INPUT_CHANGED, RECALC  } from "./actions"
import CalcPaye from "./calcPaye"


function inputs(state, action) {
  var res = state;

  switch (action.type) {
    case INPUT_CHANGED:
      res = res.set(action.field, parseFloat(action.value));
    case RECALC:
      res = CalcPaye(res);
  }

  return res;
}







export const calcAppReducer = combineReducers({
  inputs
})