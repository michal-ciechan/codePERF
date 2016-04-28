import * as Immutable from "immutable";
import { Map } from "immutable";

export class Fields {
    public static get SALARY(): string { return "Annual Salary"; }
    public static get BONUS(): string { return "Bonus"; }
    public static get BONUS_CALC(): string { return "Bonus Value"; }
    public static get PENSION(): string { return "Employer Pension Contribution"; }
    public static get PENSION_CALC(): string { return "Employer Pension Contribution Value"; }
    public static get GROSS_PAY(): string { return "Gross Pay"; }
    public static get TAXABLE_PAY(): string { return "Taxable Pay"; }
    public static get TAX_PA_LIMIT(): string { return "Tax Personal Allowance"; }
    public static get TAX_PA_CALC(): string { return "Tax Personal Amount"; }
    public static get BANK_HOLIDAYS(): string { return "Bank Holiday Days"; }
    public static get HOLIDAYS(): string { return "Holiday Days"; }
    public static get SICK_DAYS(): string { return "Sick Days"; }
    public static get WORKING_DAYS(): string { return "Working Days"; }
}

export class Actions {
    public static get INPUT_CHANGED(): string { return "INPUT_CHANGED"; }
    public static get RECALC(): string { return "RECALC"; }
}


var a = Immutable.fromJS({});


export class Defaults {
    public static get PayeTaxBands(): Immutable.Map<string, string> { return a; }
    
}

export class PayeTaxBands  {
}