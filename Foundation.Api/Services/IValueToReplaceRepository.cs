namespace Foundation.Api.Services
{
    using Foundation.Api.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IValueToReplaceRepository
    {
        Task<ValueToReplace> GetValueToReplaceAsync(int valueToReplaceId);
        ValueToReplace GetValueToReplace(int valueToReplaceId);
        IEnumerable<ValueToReplace> GetValueToReplaces();
    }
}
