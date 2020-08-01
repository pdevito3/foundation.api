namespace Infrastructure.Persistence.Repositories
{
    using Application.Dtos.ValueToReplace;
    using Application.Interfaces.ValueToReplace;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ValueToReplaceRepository : IValueToReplaceRepository
    {
        private ValueToReplaceDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public ValueToReplaceRepository(ValueToReplaceDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public PagedList<ValueToReplace> GetValueToReplaces(ValueToReplaceParametersDto valueToReplaceParameters)
        {
            if (valueToReplaceParameters == null)
            {
                throw new ArgumentNullException(nameof(valueToReplaceParameters));
            }

            var collection = _context.ValueToReplaces as IQueryable<ValueToReplace>;

            if (!string.IsNullOrWhiteSpace(valueToReplaceParameters.QueryString))
            {
                var QueryString = valueToReplaceParameters.QueryString.Trim();
                collection = collection.Where(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceTextField1.Contains(QueryString)
                    || lambdaInitialsToReplace.ValueToReplaceTextField2.Contains(QueryString));
            }

            var sieveModel = new SieveModel
            {
                Sorts = valueToReplaceParameters.SortOrder,
                Filters = valueToReplaceParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return PagedList<ValueToReplace>.Create(collection,
                valueToReplaceParameters.PageNumber,
                valueToReplaceParameters.PageSize);
        }

        public async Task<ValueToReplace> GetValueToReplaceAsync(int valueToReplaceId)
        {
            return await _context.ValueToReplaces.FirstOrDefaultAsync(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceId == valueToReplaceId);
        }

        public ValueToReplace GetValueToReplace(int valueToReplaceId)
        {
            return _context.ValueToReplaces.FirstOrDefault(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceId == valueToReplaceId);
        }

        public void AddValueToReplace(ValueToReplace valueToReplace)
        {
            if (valueToReplace == null)
            {
                throw new ArgumentNullException(nameof(valueToReplace));
            }

            _context.ValueToReplaces.Add(valueToReplace);
        }

        public void DeleteValueToReplace(ValueToReplace valueToReplace)
        {
            if (valueToReplace == null)
            {
                throw new ArgumentNullException(nameof(valueToReplace));
            }

            _context.ValueToReplaces.Remove(valueToReplace);
        }

        public void UpdateValueToReplace(ValueToReplace valueToReplace)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
