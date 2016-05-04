import Immutable, {List, Map} from "immutable";

import { FIELDS, get } from "./actions.js"
import { CalcPAYEIncomeTaxBands } from "./bands/incomePAYE"
import { CalcEmployeeNI } from "./bands/employeeNI"
import { CalcEmployerNI } from "./bands/employerNI"

export function CalcPersonalAllowance(income){
  if(income <= 100000) return 11000;

  var incomePastLimit = income - 100000;

  var deduction = Math.floor(incomePastLimit / 2);

  var personalAllowanceLessDeduction = Math.max(0, 11000 - deduction);

  return personalAllowanceLessDeduction;
}

function CalcBonus(inputs) {
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS);

  var res = salary * (bonus / 100);

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
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS_CALC);
  var pension = get(inputs, FIELDS.PENSION_CALC);

  var res = salary + bonus + pension;

  return inputs.set(FIELDS.GROSS_PAY, res);
}

function CalcTaxablePay(inputs) {
  var salary = get(inputs, FIELDS.SALARY);
  var bonus = get(inputs, FIELDS.BONUS_CALC);

  var res = salary + bonus;

  return inputs.set(FIELDS.TAXABLE_PAY, res);
}

function CalcAdjustedPersonalAllowance(inputs) {
  var pay = get(inputs, FIELDS.TAXABLE_PAY);

  var res = CalcPersonalAllowance(pay);

  return inputs.set(FIELDS.ADJUSTED_PERSONAL_ALLOWANCE, res);
}

function CalcTotalEmployeeCosts(inputs) {
  var pay = get(inputs, FIELDS.TAXABLE_PAY);
  var pension = get(inputs, FIELDS.PENSION_CALC);
  var tax = get(inputs, FIELDS.PAYE_TAX_BANDS_TOTAL);
  var ni = get(inputs, FIELDS.EMPLOYEE_NI_TOTAL);
  var days = get(inputs, FIELDS.WORKING_DAYS);

  var net = pay - tax - ni;
  var total = net + pension;
  var daily = total / days;

  return inputs
              .set(FIELDS.TOTAL_NET_INCOME, net)
              .set(FIELDS.TOTAL_INCOME, total)
              .set(FIELDS.DAILY_NET_INCOME, daily);
}

function CalcTotalEmployerCosts(inputs) {
  var pay = get(inputs, FIELDS.GROSS_PAY);
  var ni = get(inputs, FIELDS.EMPLOYER_NI_TOTAL);
  var days = get(inputs, FIELDS.WORKING_DAYS);

  var res = pay + ni;
  var daily = res / days;

  return inputs
              .set(FIELDS.TOTAL_COST_TO_EMPLOYER, res)
              .set(FIELDS.DAILY_COST_TO_EMPLOYER, daily);
}

function CalcContractVAT(inputs) {
    var vatRate = 20;
    var rate = get(inputs, FIELDS.VAT_FLAT_RATE);
    var pay = get(inputs, FIELDS.TOTAL_COST_TO_EMPLOYER);
    var vatExpenses = get(inputs, FIELDS.VAT_EXPENSES);
    var expenses = get(inputs, FIELDS.EXPENSES);

    var incomeRate = vatRate - rate;
    var income = pay * (incomeRate / 100);
    var reclaim = vatExpenses * (vatRate / 100);
    var newExpenses = expenses + vatExpenses - reclaim;

    return inputs
                .set(FIELDS.VAT_INCOME_RATE, incomeRate)
                .set(FIELDS.VAT_INCOME, income)
                .set(FIELDS.VAT_RECLAIM, reclaim)
                .set(FIELDS.EXPENSES, newExpenses);
}

function CalcContractExpenses(inputs) {
    var pay = get(inputs, FIELDS.TOTAL_COST_TO_EMPLOYER);


    return inputs
}

function Reset(inputs) {
    return inputs.set(FIELDS.EXPENSES, 0);
}

export default function CalcPaye(inputs) {
  var res = Reset(inputs);

  res = CalcBonus(res);
  res = CalcPension(res);
  res = CalcGrossPay(res);
  res = CalcTaxablePay(res);

  res = CalcWorkingDays(res);
  res = CalcAdjustedPersonalAllowance(res);
  res = CalcPAYEIncomeTaxBands(res);

  res = CalcEmployeeNI(res);
  res = CalcEmployerNI(res);

  res = CalcTotalEmployeeCosts(res);
  res = CalcTotalEmployerCosts(res);

  res = CalcContractVAT(res);
  res = CalcContractExpenses(res);

  return res;
}
