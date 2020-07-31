namespace Application.Interfaces.ValueToReplace
{
    using Application.Dtos.ValueToReplace;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IValueToReplaceRepository
    {
        PagedList<ValueToReplace> GetValueToReplaces(ValueToReplaceParametersDto valueToReplaceParameters);
        Task<ValueToReplace> GetValueToReplaceAsync(int valueToReplaceId);
        ValueToReplace GetValueToReplace(int valueToReplaceId);
        void AddValueToReplace(ValueToReplace valueToReplace);
        void DeleteValueToReplace(ValueToReplace valueToReplace);
        void UpdateValueToReplace(ValueToReplace valueToReplace);
        bool Save();
    }
}
