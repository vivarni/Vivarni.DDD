using System;
using Vivarni.Domain.Core.Time;

namespace Vivarni.Domain.Infrastructure
{
    public class DateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
