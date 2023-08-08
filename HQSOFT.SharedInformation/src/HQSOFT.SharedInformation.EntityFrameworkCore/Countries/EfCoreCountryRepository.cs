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

namespace HQSOFT.SharedInformation.Countries
{
    public class EfCoreCountryRepository : EfCoreRepository<SharedInformationDbContext, Country, Guid>, ICountryRepository
    {
        public EfCoreCountryRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Country>> GetListAsync(
            string filterText = null,
            string code = null,
            string description = null,
            string dateFormat = null,
            string timeFormat = null,
            string timeZone = null,
            int? idxMin = null,
            int? idxMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, description, dateFormat, timeFormat, timeZone, idxMin, idxMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CountryConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string description = null,
            string dateFormat = null,
            string timeFormat = null,
            string timeZone = null,
            int? idxMin = null,
            int? idxMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, code, description, dateFormat, timeFormat, timeZone, idxMin, idxMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Country> ApplyFilter(
            IQueryable<Country> query,
            string filterText,
            string code = null,
            string description = null,
            string dateFormat = null,
            string timeFormat = null,
            string timeZone = null,
            int? idxMin = null,
            int? idxMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code.ToLower().Contains(filterText.ToLower()) || e.Description.ToLower().Contains(filterText.ToLower()) || e.DateFormat.ToLower().Contains(filterText.ToLower()) || e.TimeFormat.ToLower().Contains(filterText.ToLower()) || e.TimeZone.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.ToLower().Contains(code.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.ToLower().Contains(description.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(dateFormat), e => e.DateFormat.ToLower().Contains(dateFormat.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(timeFormat), e => e.TimeFormat.ToLower().Contains(timeFormat.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(timeZone), e => e.TimeZone.ToLower().Contains(timeZone.ToLower()))
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value);
        }
    }
}