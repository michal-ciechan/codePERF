



export function CalcBands(bands, value) {
    var res = bands.withMutations((list) => {
      var total = 0;
      var remaining = value;

      bands.forEach((curValue, key, iterator) => {
        var bandLimit = curValue.get("Limit");
        var rate = curValue.get("Rate");
        var limit = bandLimit - total;

        var use = Math.min(limit, remaining);

        remaining = remaining - use;
        total = total + use;

        var tax = use * (rate / 100);

        var newValue = curValue.set("Value", use).set("Tax", tax);

        list.set(key, newValue);

      return true;
    })
  });

  //console.log(res.get(1).get("Value"));
  return res;
}
