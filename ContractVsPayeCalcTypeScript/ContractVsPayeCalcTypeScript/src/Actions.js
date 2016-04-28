"use strict";
const Immutable = require("immutable");
class Fields {
    static get SALARY() { return "Annual Salary"; }
    static get BONUS() { return "Bonus"; }
    static get BONUS_CALC() { return "Bonus Value"; }
    static get PENSION() { return "Employer Pension Contribution"; }
    static get PENSION_CALC() { return "Employer Pension Contribution Value"; }
    static get GROSS_PAY() { return "Gross Pay"; }
    static get TAXABLE_PAY() { return "Taxable Pay"; }
    static get TAX_PA_LIMIT() { return "Tax Personal Allowance"; }
    static get TAX_PA_CALC() { return "Tax Personal Amount"; }
    static get BANK_HOLIDAYS() { return "Bank Holiday Days"; }
    static get HOLIDAYS() { return "Holiday Days"; }
    static get SICK_DAYS() { return "Sick Days"; }
    static get WORKING_DAYS() { return "Working Days"; }
}
exports.Fields = Fields;
class Actions {
    static get INPUT_CHANGED() { return "INPUT_CHANGED"; }
    static get RECALC() { return "RECALC"; }
}
exports.Actions = Actions;
var a = Immutable.fromJS({});
class Defaults {
    static get PayeTaxBands() { return a; }
}
exports.Defaults = Defaults;
class PayeTaxBands {
}
exports.PayeTaxBands = PayeTaxBands;
