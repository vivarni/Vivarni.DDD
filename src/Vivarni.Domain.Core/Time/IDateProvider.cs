using System;

namespace Vivarni.Domain.Core.Time
{
    public interface IDateProvider
    {
        DateTime Today { get; }
        DateTimeOffset Now { get; }
    }
}
