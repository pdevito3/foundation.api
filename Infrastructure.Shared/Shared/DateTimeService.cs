namespace Infrastructure.Shared.Shared
{
    using Application.Interfaces;
    using System;

    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
