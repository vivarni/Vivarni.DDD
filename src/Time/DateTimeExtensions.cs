using System;

namespace Vivarni.Domain.Core.Util
{
    public static class DateTimeExtensions
    {
        public static DateTime Add(this DateTime @this, DateSpan span)
        {
            if (span.Unit == DateSpan.DateSpanUnit.Day)
                return @this.AddDays(span.Value);
            if (span.Unit == DateSpan.DateSpanUnit.Month)
                return @this.AddMonths(span.Value);
            if (span.Unit == DateSpan.DateSpanUnit.Year)
                return @this.AddYears(span.Value);

            throw new NotImplementedException();
        }

        public static DateTimeOffset Add(this DateTimeOffset @this, DateSpan span)
        {
            if (span.Unit == DateSpan.DateSpanUnit.Day)
                return @this.AddDays(span.Value);
            if (span.Unit == DateSpan.DateSpanUnit.Month)
                return @this.AddMonths(span.Value);
            if (span.Unit == DateSpan.DateSpanUnit.Year)
                return @this.AddYears(span.Value);

            throw new NotImplementedException();
        }
    }
}
