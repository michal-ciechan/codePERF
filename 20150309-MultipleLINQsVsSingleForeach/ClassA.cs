using System;
using System.Globalization;

namespace MultipleLINQsVsSingleForeach
{
    public class ClassA
    {
        private static readonly Random _r = new Random();

        public ClassA()
        {
            Int1 = _r.Next();
            String1 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String2 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String3 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String4 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String5 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String6 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String7 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String8 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
            String9 = _r.NextDouble().ToString(CultureInfo.InvariantCulture);
        }

        public int Int1 { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        public string String6 { get; set; }
        public string String7 { get; set; }
        public string String8 { get; set; }
        public string String9 { get; set; }
    }
}