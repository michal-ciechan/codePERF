import { CalcBands } from '../src/calcPaye'
import { PayeTaxBands } from '../src/actions'
//import expect, { createSpy, spyOn, isSpy } from 'expect'

import Immutable, {List, Map} from 'immutable';
import chai, {expect} from 'chai';
import chaiImmutable from 'chai-immutable';
chai.use(chaiImmutable);

import deepFreeze from 'deep-freeze'

import Enumerable from 'linq'



describe('CalcBands', function() {
  
  describe('PAYE Bands', function () {
    
    
    it('PayeTaxBands should be Immutabl.List', function () {
      expect(PayeTaxBands).to.be.instanceOf(List);
    });
    
    it('PayeTaxBands should only contain Immutable.Maps', function () {
      for (var band of PayeTaxBands){
          expect(band).to.be.instanceOf(Map);
      }
    });
    
    it('should return Immutable.List', function () {      
      var res = CalcBands(PayeTaxBands, 0);
      
      expect(res).to.be.instanceOf(List);
    });
    
    it('should return List containing only Immutable.Maps', function () {
      var res = CalcBands(PayeTaxBands, 0);
      
      for (var band of res){
          expect(band).to.be.instanceOf(Map);
      }
    });
    
    it('should return all PayeTaxBands', function () {
      var res = CalcBands(PayeTaxBands, 0)
            
      for (var band of res){
        expect(res).to.include(band)        
      }      
    });
    
    it('should return all bands with value', function () {
      var res = CalcBands(PayeTaxBands, 0)
            
      for (var band of res){
        expect(band).to.include.key("Value");        
      }      
    });
    
    it("first band should be Personal Allowance", function () {
      var res = CalcBands(PayeTaxBands, 0)
            
      expect(res.first()).to.include.key({"Band": "Personal Allowance"})  
    });
    
    it('with 11k income should return Personal Allowance band with 11k', function () {
      // var result = CalcBands(PayeTaxBands, 11000)
      
      // var r = result.values();
      
      // expect(r).to.be.equal(2)
      
    });
    
    List.prototype.Sum = function(x) {
      var sum = 0;
      
      this.forEach((val) => {
        console.log(val);
        sum += x(val);
      });
      
      return sum;      
    }
    
    it('with 11k income should return sum of all allowances as 0', function () {
      var result = CalcBands(PayeTaxBands, 11000)
      
      console.log(result);
      
      var sum = 0;
      
      var a = result.Sum((x) => x.get("Value"));
      
      
      
      console.log(a)
      
      
      console.log(sum);
      
      
      
      expect(sum).to.equal(110);
    });
    
    
    it('with 11k income should return Personal Allowance band with 11k', function () {
      var res = CalcBands(PayeTaxBands, 11000)
      
      //expect(res.first().get("Value")).to.be.equal(11000)
    });
  });
  
});