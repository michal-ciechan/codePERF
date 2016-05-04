import Immutable, {List} from "immutable";
import { CalcBands } from "./shared"
import { FIELDS, get } from "../actions.js"

List.prototype.Sum = function(x) {
  var sum = 0;

  this.forEach((val) => {
    sum += x(val);
  });

  return sum;
}

export const TaxBandsEmployerNI = Immutable.fromJS([
  {
    Band: "Free",
    Limit: 676 * 12,
    Rate: 0
  },
  {
    Band: "Basic Rate",
    Limit: 3583 * 12,
    Rate: 13.8
  },
  {
    Band: "Higher Rate",
    Limit: Number.MAX_SAFE_INTEGER,
    Rate: 13.8
  },
]);

export function CalcEmployerNI(inputs) {

  var pay = get(inputs, FIELDS.TAXABLE_PAY);
  var bands = TaxBandsEmployerNI;

  var res = CalcBands(bands, pay);
  var sum = res.Sum((x) => x.get("Tax"));

  return inputs
          .set(FIELDS.EMPLOYER_NI, res)
          .set(FIELDS.EMPLOYER_NI_TOTAL, sum);
}
