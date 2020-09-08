namespace Infrastructure.Shared.Services
{
    using Application.Interfaces;
    using System;

    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
