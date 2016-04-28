import React from 'react'
import { combineReducers } from 'redux'
import { connect } from 'react-redux'

import { formats } from '../utilities'

import { InputChanged } from '../actions'
import { FIELDS  } from '../actions'

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
    var value = getField(state, field);
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


const PayeFieldsView = () => (
    <div>
        <Input field={FIELDS.SALARY} format={formats.Currency} />
        <Input field={FIELDS.BONUS} format={formats.Percent} />
        <Output field={FIELDS.BONUS_CALC}  format={formats.Currency} />
        <Input field={FIELDS.PENSION} format={formats.Percent} />
        <Output field={FIELDS.PENSION_CALC}  format={formats.Currency} />
        <Output field={FIELDS.GROSS_PAY} format={formats.Currency} />
        <Output field={FIELDS.TAXABLE_PAY} format={formats.Currency} />
        <Input field={FIELDS.BANK_HOLIDAYS} />
        <Input field={FIELDS.HOLIDAYS} />
        <Input field={FIELDS.SICK_DAYS} />
        <Output field={FIELDS.WORKING_DAYS} />
    </div>
);

export default connect()(PayeFieldsView)