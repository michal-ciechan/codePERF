import Immutable, {List, Map} from 'immutable';

import { FIELDS } from './actions.js'
import { PayeTaxBands } from './actions.js'


function get(inputs, field){
  return inputs.get(field) || 0;
}

export function CalcBands(bands, value) {
    var res = bands.withMutations((list) => {
      bands.forEach((value, key, iterator) => {
        list.set(key, value.set('Value', 0))
      return true;
    })   
  });
     
  
  return res;
}

function CalcBonus(inputs) {
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS);
  
  var res = salary * (bonus / 100)

  return inputs.set(FIELDS.BONUS_CALC, res);
}

function CalcPension(inputs) {
  var salary = get(inputs, FIELDS.SALARY);
  var pension = get(inputs, FIELDS.PENSION);
  
  var res = salary * (pension / 100)

  return inputs.set(FIELDS.PENSION_CALC, res);
}

function CalcWorkingDays(inputs) {
  var base = 260;
  
  var bankHolidays = get(inputs, FIELDS.BANK_HOLIDAYS);
  var holidays = get(inputs, FIELDS.HOLIDAYS);
  var sickDays = get(inputs, FIELDS.SICK_DAYS);
  
  var res = base - bankHolidays - holidays - sickDays;
  
  return inputs.set(FIELDS.WORKING_DAYS, res);
}

function CalcGrossPay(inputs) {
  var base = 260;
  
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS_CALC);
  var pension = get(inputs, FIELDS.PENSION_CALC);
  
  var res = salary + bonus + pension;
  
  return inputs.set(FIELDS.GROSS_PAY, res);
}

function CalcTaxablePay(inputs) {
  var base = 260;
  
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS_CALC);
  
  var res = salary + bonus;
  
  return inputs.set(FIELDS.TAXABLE_PAY, res);
}

export default function CalcPaye(inputs) {
  var res = inputs;

  res = CalcBonus(res);
  res = CalcPension(res);
  res = CalcGrossPay(res);
  res = CalcTaxablePay(res);
  
  res = CalcWorkingDays(res);

  return res;
}