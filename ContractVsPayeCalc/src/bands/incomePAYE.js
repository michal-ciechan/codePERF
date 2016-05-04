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

export const TaxBandsPAYE = Immutable.fromJS([
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

export function CalcPAYEIncomeTaxBands(inputs) {

  var pay = get(inputs, FIELDS.TAXABLE_PAY);
  var allowance = get(inputs, FIELDS.ADJUSTED_PERSONAL_ALLOWANCE);
  var bands = TaxBandsPAYE.set(0, TaxBandsPAYE.first().set("Limit", allowance))

  var res = CalcBands(bands, pay);
  var sum = res.Sum((x) => x.get("Tax"));

  return inputs
          .set(FIELDS.PAYE_TAX_BANDS, res)
          .set(FIELDS.PAYE_TAX_BANDS_TOTAL, sum);
}
