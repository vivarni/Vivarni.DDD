using System;

namespace Vivarni.Domain.Core
{
    public interface IDateProvider
    {
        DateTime Today { get; }
        DateTimeOffset Now { get; }
    }
}
