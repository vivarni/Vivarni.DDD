using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Vivarni.Domain.Core.Time.Exceptions;

namespace Vivarni.Domain.Core.Time
{
    [JsonConverter(typeof(DateSpanConverter))]
    public class DateSpan
    {
        public DateSpanUnit Unit { get; }
        public int Value { get; }

        private static readonly Regex _format = new Regex(@"([+\-0-9]+)([DMY])");

        public enum DateSpanUnit
        {
            Day, Month, Year
        }

        public DateSpan(DateSpanUnit unit, int value)
        {
            Unit = unit;
            Value = value;
        }

        public static DateSpan Zero = new DateSpan(DateSpanUnit.Day, 0);

        public static DateSpan FromDays(int value)
        {
            return new DateSpan(DateSpanUnit.Day, value);
        }

        public static DateSpan FromMonths(int value)
        {
            return new DateSpan(DateSpanUnit.Month, value);
        }

        public static DateSpan FromYears(int value)
        {
            return new DateSpan(DateSpanUnit.Year, value);
        }

        public static DateSpan FromString(string value)
        {
            var match = _format.Matches(value).FirstOrDefault();
            if (match == null)
                throw new InvalidDateSpanFormatException();

            var m1 = match.Groups[1].Value;
            var m2 = match.Groups[2].Value;

            var v = int.Parse(m1);
            var u =
                m2 == "D" ? DateSpanUnit.Day :
                m2 == "M" ? DateSpanUnit.Month :
                m2 == "Y" ? DateSpanUnit.Year :
                throw new InvalidDateSpanFormatException();

            return new DateSpan(u, v);
        }

        public override string ToString()
        {
            var unit =
                Unit == DateSpanUnit.Day ? "D" :
                Unit == DateSpanUnit.Month ? "M" :
                Unit == DateSpanUnit.Year ? "Y" :
                throw new NotImplementedException();

            return $"{Value}{unit}";
        }
    }
}
