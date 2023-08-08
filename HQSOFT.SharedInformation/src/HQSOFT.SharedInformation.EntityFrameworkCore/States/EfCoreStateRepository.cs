using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.SharedInformation.EntityFrameworkCore;

namespace HQSOFT.SharedInformation.States
{
    public class EfCoreStateRepository : EfCoreRepository<SharedInformationDbContext, State, Guid>, IStateRepository
    {
        public EfCoreStateRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<State>> GetListAsync(
            string filterText = null,
            Guid? countryId = null,
            int? idxMin = null,
            int? idxMax = null,
            string stateCode = null,
            string stateName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, countryId, idxMin, idxMax, stateCode, stateName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? StateConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? countryId = null,
            int? idxMin = null,
            int? idxMax = null,
            string stateCode = null,
            string stateName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, countryId, idxMin, idxMax, stateCode, stateName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<State> ApplyFilter(
            IQueryable<State> query,
            string filterText,
            Guid? countryId = null,
            int? idxMin = null,
            int? idxMax = null,
            string stateCode = null,
            string stateName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.StateCode.ToLower().Contains(filterText.ToLower()) || e.StateName.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(countryId.HasValue, e => e.CountryId == countryId)
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(stateCode), e => e.StateCode.ToLower().Contains(stateCode.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(stateName), e => e.StateName.ToLower().Contains(stateName.ToLower()));
        }
    }
}