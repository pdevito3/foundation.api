namespace Application.Interfaces
{
    using System;

    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
