import { CalcPersonalAllowance } from "../src/calcPaye"
import { CalcBands } from "../src/bands/shared"
import { TaxBandsPAYE } from "../src/bands/incomePAYE"
//import expect, { createSpy, spyOn, isSpy } from 'expect'

import Immutable, {List, Map} from "immutable";
import chai, {expect} from "chai";
import chaiImmutable from "chai-immutable";
chai.use(chaiImmutable);

import deepFreeze from "deep-freeze"

import Enumerable from "linq"


List.prototype.Sum = function(x) {
  var sum = 0;

  this.forEach((val) => {
    sum += x(val);
  });

  return sum;
}

describe("CalcPersonalAllowance", function() {
  it("50k income should return 11k Personal Allowance", function() {
    var income = 50000;

    var res = CalcPersonalAllowance(income);

    expect(res).to.equal(11000);
  });
});

describe("CalcPersonalAllowance", function() {
  it("130k income should return 11k Personal Allowance", function() {
    var income = 130000;

    var res = CalcPersonalAllowance(income);

    expect(res).to.equal(0);
  });
});

describe("CalcPersonalAllowance", function() {
  it("130k income should return 11k Personal Allowance", function() {
    var income = 120000;

    var res = CalcPersonalAllowance(income);

    expect(res).to.equal(1000);
  });
});

describe("CalcBands", function() {

  describe("PAYE Bands", function () {
    it("TaxBandsPAYE should be Immutabl.List", function () {
      expect(TaxBandsPAYE).to.be.instanceOf(List);
    });

    it("TaxBandsPAYE should only contain Immutable.Maps", function () {
      for (var band of TaxBandsPAYE){
          expect(band).to.be.instanceOf(Map);
      }
    });

    it("should return Immutable.List", function () {
      var res = CalcBands(TaxBandsPAYE, 0);

      expect(res).to.be.instanceOf(List);
    });

    it("should return List containing only Immutable.Maps", function () {
      var res = CalcBands(TaxBandsPAYE, 0);

      for (var band of res){
          expect(band).to.be.instanceOf(Map);
      }
    });

    it("should return all TaxBandsPAYE", function () {
      var res = CalcBands(TaxBandsPAYE, 0)

      for (var band of res){
        expect(res).to.include(band)
      }
    });

    it("should return all bands with value", function () {
      var res = CalcBands(TaxBandsPAYE, 0)

      for (var band of res){
        expect(band).to.include.key("Value");
      }
    });

    it("first band should be Personal Allowance", function () {
      var res = CalcBands(TaxBandsPAYE, 0)

      expect(res.first()).to.include.key({"Band": "Personal Allowance"})
    });

    it("with 11k income should return Personal Allowance band with 11k", function () {
      var result = CalcBands(TaxBandsPAYE, 11000)

      var first = result.first().get("Value");

      expect(first).to.be.equal(11000)

    });

    it("with 20k income should return second band with 9k", function () {
      var result = CalcBands(TaxBandsPAYE, 20000)

      var res = result.get(1).get("Value");

      expect(res).to.be.equal(9000)

    });

    it("with 47k income should return sum of all allowances as 47k", function () {
      var result = CalcBands(TaxBandsPAYE, 47000);

      var sum = result.Sum((x) => x.get("Value"));

      expect(sum).to.equal(47000);
    });

    it("with 200k income should return last band (150k) with 50k value", function () {
      var result = CalcBands(TaxBandsPAYE, 200000)

      var band = result.last().get("Band");
      expect(band).to.equal("Additional Rate");

      var v = result.last().get("Value");

      expect(v).to.equal(50000);
    });

    it("with 11k income should return Personal Allowance band with 11k", function () {
      var res = CalcBands(TaxBandsPAYE, 11000)

      //expect(res.first().get("Value")).to.be.equal(11000)
    });

    it("with 11k income should return tax of 0", function () {
      var res = CalcBands(TaxBandsPAYE, 11000)

      var tax = res.Sum((x) => x.get("Tax"));

      expect(tax).to.be.equal(0);
    });

    it("with 43k income should return tax of 6.4k", function () {
      var res = CalcBands(TaxBandsPAYE, 43000)

      var tax = res.Sum((x) => x.get("Tax"));

      expect(tax).to.be.equal(6400);
    });

    it("with 150k income should return tax of 49.2k", function () {
      var res = CalcBands(TaxBandsPAYE, 150000)

      var tax = res.Sum((x) => x.get("Tax"));

      expect(tax).to.be.equal(49200);
    });


  });
});
