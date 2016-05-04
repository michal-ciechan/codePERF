import React from "react"
import { combineReducers } from "redux"
import { connect } from "react-redux"

import { formats } from "../utilities"

import { InputChanged } from "../actions"
import { FIELDS  } from "../actions"
import Immutable, {List, Map} from "immutable";



const getField = (state, field) => {
    var inputs = state.get("inputs");

    if(inputs == null) return "";

    if(!inputs.has(field)){
      return "";
    }

    return state.get("inputs").get(field);
}

const mapDispatchToProps = (dispatch) => {
    return {
        onChange: (value, field) => {
            dispatch(InputChanged(value, field))
        }
    }
}

// Input
const mapStateToPropsInput = (state, props) => {
    var field = props.field;
    var value = props.value || getField(state, field);
    var format = props.format || formats.Integer;

    var res = {
        title: props.title || field,
        value: value,
        formattedValue: format(value),
        field: field,
    }

    return res;
};
const InputView = ({ title, value, formattedValue, field, onChange }) => (
    <div>
        <span className="key">{title}: </span>
        <input type="text"
            placeholder={title}
            value={value}
            onChange={(event) => onChange(event.target.value, field) } />
        <span className="value">{formattedValue}</span>
    </div>
);
export const Input = connect(
    mapStateToPropsInput,
    mapDispatchToProps)(InputView);

// Output
const OutputView = ({ title, value, formattedValue, field }) => (
    <div>
        <span className="key">{title}: </span>
        <span className="output-gap" />
        <span className="value">{formattedValue}</span>
    </div>
);
const Output = connect(mapStateToPropsInput)
(OutputView);

// Band
const mapStateToPropsBand = (state, props) => {
  var band = props.band;

    return {
        title: band.get("Band"),
        tax: band.get("Tax"),
    }
};
const BandView = ({ title, tax, visible }) => (
  <Output title={title} value={tax} format={formats.Currency} />
);
const Band = connect(mapStateToPropsBand)(BandView);

// Bands
const mapStateToPropsBands = (state, props) => {
var totalField = props.totalField || props.field + " Total";


  var bands = getField(state, props.field);
  var total = getField(state, totalField );
  var filteredBands = bands.filter(b => b.get("Tax") > 0);

    return {
        total: total,
        title: props.title || props.field,
        bands: filteredBands,
    }
};
const BandsView = ({ title, total, bands }) => (
    <div>
      <Output title={title} value={total} format={formats.Currency} />
    {bands.map(band => <Band key={band.get("Band")} band={band} />)}
    </div>
);
const Bands = connect(mapStateToPropsBands)(BandsView);


// Component
const PayeFieldsView = () => (
    <div>
      <Input field={FIELDS.SALARY} format={formats.Currency} />
      <Input field={FIELDS.BONUS} format={formats.Percent} />
      <Output field={FIELDS.BONUS_CALC}  format={formats.Currency} />
      <Output field={FIELDS.GROSS_PAY} format={formats.Currency} />
      <Output field={FIELDS.TAXABLE_PAY} format={formats.Currency} />
      <Output field={FIELDS.ADJUSTED_PERSONAL_ALLOWANCE} format={formats.Currency} />
      <Bands field={FIELDS.PAYE_TAX_BANDS} />
      <Bands field={FIELDS.EMPLOYEE_NI} />
      <Output field={FIELDS.TOTAL_NET_INCOME}  format={formats.Currency} />
      <Bands field={FIELDS.EMPLOYER_NI} />
      <Input field={FIELDS.PENSION} format={formats.Percent} />
      <Output field={FIELDS.PENSION_CALC}  format={formats.Currency} />
      <Output field={FIELDS.TOTAL_COST_TO_EMPLOYER}  format={formats.Currency} />
      <Output field={FIELDS.TOTAL_INCOME}  format={formats.Currency} />
      <Input field={FIELDS.BANK_HOLIDAYS} />
      <Input field={FIELDS.HOLIDAYS} />
      <Input field={FIELDS.SICK_DAYS} />
      <Output field={FIELDS.WORKING_DAYS} />
      <Output field={FIELDS.DAILY_NET_INCOME}  format={formats.Currency} />
      <Output field={FIELDS.DAILY_COST_TO_EMPLOYER}  format={formats.Currency} />
      <Input field={FIELDS.VAT_FLAT_RATE} format={formats.Percent} />
      <Output field={FIELDS.VAT_INCOME_RATE} format={formats.Percent} />
      <Output field={FIELDS.VAT_INCOME} format={formats.Currency} />
      <Input field={FIELDS.VAT_EXPENSES} format={formats.Currency} />
      <Output field={FIELDS.VAT_RECLAIM} format={formats.Currency} />
      <Output field={FIELDS.EXPENSES} format={formats.Currency} />
    </div>
);

export default connect()(PayeFieldsView)
