namespace Foundation.Api.Services
{
    using Foundation.Api.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IValueToReplaceRepository
    {
        IEnumerable<ValueToReplace> GetValueToReplaces();
        Task<ValueToReplace> GetValueToReplaceAsync(int valueToReplaceId);
        ValueToReplace GetValueToReplace(int valueToReplaceId);
        void AddValueToReplace(ValueToReplace valueToReplace);
        void DeleteValueToReplace(ValueToReplace valueToReplace);
        void UpdateValueToReplace(ValueToReplace valueToReplace);
        bool Save();
    }
}
