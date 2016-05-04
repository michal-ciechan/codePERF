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
  ADJUSTED_PERSONAL_ALLOWANCE: "Adjusted Personal Allowance",
  PAYE_TAX_BANDS: "PAYE Tax",
  PAYE_TAX_BANDS_TOTAL: "PAYE Tax Total",
  EMPLOYEE_NI: "Emplyee National Insurance",
  EMPLOYEE_NI_TOTAL: "Emplyee National Insurance Total",
  EMPLOYER_NI: "Employer National Insurance",
  EMPLOYER_NI_TOTAL: "Employer National Insurance Total",
  TOTAL_COST_TO_EMPLOYER: "Total Cost to Employer",
  TOTAL_NET_INCOME: "Total Net Income",
  TOTAL_INCOME: "Total Income",
  DAILY_COST_TO_EMPLOYER: "Daily Cost to Employer",
  DAILY_NET_INCOME: "Daily Net Income",
  VAT_FLAT_RATE: "VAT Flat Rate",
  VAT_INCOME_RATE: "VAT Income Rate",
  VAT_INCOME: "VAT Income",
  VAT_RECLAIM: "VAT Reclaim",
  VAT_EXPENSES: "VAT-able Expenses",
  VAT_RECLAIM: "VAT Reclaim",
  EXPENSES: "Expenses",
  TRAVEL_EXPENSES: "Travel Expenses",
  NETWORKING_EXPENSES: "Networking Expenses",
  OFFICE_EXPENSES: "Office Expenses",
  OTHER_EXPENSES: "Other Expenses",
  COMPANY_PROFIT: "Company Profit",
  DIRECTORS_PAY: "Directors Pay",
  DIVIDEND_TAX: "Dividend Tax",
  DIVIDEND_TAX_TOTAL: "Dividend Tax Total",
  DIRECTOR_NET_INCOME: "Director Net Income",
  DIRECTOR_DAILY_NET_INCOME: "Contractor Daily Net Income",
};

export function get(inputs, field){
  return inputs.get(field) || 0;
}



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
