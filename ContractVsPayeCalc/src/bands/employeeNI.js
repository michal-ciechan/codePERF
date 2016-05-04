import Immutable, {List} from "immutable";
import { CalcBands } from "./shared"
import { FIELDS, get } from "../actions.js"
import { PayeTaxBands } from "../actions.js"

List.prototype.Sum = function(x) {
  var sum = 0;

  this.forEach((val) => {
    sum += x(val);
  });

  return sum;
}

export const TaxBandsEmployeeNI = Immutable.fromJS([
  {
    Band: "Free",
    Limit: 672 * 12,
    Rate: 0
  },
  {
    Band: "Basic Rate",
    Limit: 3583 * 12,
    Rate: 12
  },
  {
    Band: "Higher Rate",
    Limit: Number.MAX_SAFE_INTEGER,
    Rate: 2
  },
]);

export function CalcEmployeeNI(inputs) {

  var pay = get(inputs, FIELDS.TAXABLE_PAY);
  var bands = TaxBandsEmployeeNI;

  var res = CalcBands(bands, pay);
  var sum = res.Sum((x) => x.get("Tax"));

  return inputs
          .set(FIELDS.EMPLOYEE_NI, res)
          .set(FIELDS.EMPLOYEE_NI_TOTAL, sum);
}
