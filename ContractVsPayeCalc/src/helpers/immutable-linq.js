

List.prototype.Sum = function(x) {
  var sum = 0;

  this.forEach((val) => {
    sum += x(val);
  });

  return sum;
}
