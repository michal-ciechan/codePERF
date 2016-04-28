import Immutable from "immutable";

/*
 * action types
 */

export const INPUT_CHANGED = "INPUT_CHANGED";
export const RECALC = "RECALC";

export const FIELDS = {
  SALARY: "Annual Salary",
  BONUS: "Bonus",
  BONUS_CALC: "Bonus Value",
  PENSION: "Employer Pension Contribution",
  PENSION_CALC: "Employer Pension Contribution Value",  
  GROSS_PAY: "Gross Pay",
  TAXABLE_PAY: "Taxable Pay",
  TAX_PA_LIMIT: "Tax Personal Allowance",
  TAX_PA_CALC: "Tax Personal Amount",
  BANK_HOLIDAYS: "Bank Holiday Days",
  HOLIDAYS: "Holiday Days",
  SICK_DAYS: "Sick Days",
  WORKING_DAYS: "Working Days",
};

export const PayeTaxBands = Immutable.fromJS([
  {
    Band: "Personal Allowance",
    Limit: 11000,
    Rate: 0 
  },
  {
    Band: "Basic Rate",
    Limit: 43000,
    Rate: 20 
  },
  {
    Band: "Higher Rate",
    Limit: 150000,
    Rate: 40 
  },
  {
    Band: "Additional Rate",
    Limit: Number.MAX_SAFE_INTEGER,
    Rate: 45
  },
]);

/*
 * other constants
 */

/*
 * action creators
 */


export function InputChanged(value, field) {
  return { value, field, type: INPUT_CHANGED }
}

export function ForceRecalc() {
  return { type: RECALC }
}