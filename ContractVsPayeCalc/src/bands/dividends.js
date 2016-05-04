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

export const DividendTaxBands = Immutable.fromJS([
  {
    Band: "Free",
    Limit: 11000 - 8000 + 5000, // 11K PA - 8k Directors Pay + 5k Free Divcidend
    Rate: 0
  },
  {
    Band: "Basic Rate",
    Limit: 43000 - 3000,
    Rate: 7.5
  },
  {
    Band: "Higher Rate",
    Limit:150000 - 3000,
    Rate: 32.5
  },
  {
    Band: "Higher Rate",
    Limit: Number.MAX_SAFE_INTEGER,
    Rate: 38.1
  },
]);

export function CalcDividendsTax(inputs) {

  var pay = get(inputs, FIELDS.COMPANY_PROFIT);
  var bands = DividendTaxBands;

  var res = CalcBands(bands, pay);
  var sum = res.Sum((x) => x.get("Tax"));

  return inputs
          .set(FIELDS.DIVIDEND_TAX, res)
          .set(FIELDS.DIVIDEND_TAX_TOTAL, sum);
}
