using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using HQSOFT.SharedInformation.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace HQSOFT.SharedInformation.Countries
{
    public class MongoCountryRepository : MongoDbRepository<SharedInformationMongoDbContext, Country, Guid>, ICountryRepository
    {
        public MongoCountryRepository(IMongoDbContextProvider<SharedInformationMongoDbContext> dbContextProvider)
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, code, description, dateFormat, timeFormat, timeZone, idxMin, idxMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CountryConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<Country>>()
                .PageBy<Country, IMongoQueryable<Country>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, code, description, dateFormat, timeFormat, timeZone, idxMin, idxMax);
            return await query.As<IMongoQueryable<Country>>().LongCountAsync(GetCancellationToken(cancellationToken));
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
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code.Contains(filterText) || e.Description.Contains(filterText) || e.DateFormat.Contains(filterText) || e.TimeFormat.Contains(filterText) || e.TimeZone.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(dateFormat), e => e.DateFormat.Contains(dateFormat))
                    .WhereIf(!string.IsNullOrWhiteSpace(timeFormat), e => e.TimeFormat.Contains(timeFormat))
                    .WhereIf(!string.IsNullOrWhiteSpace(timeZone), e => e.TimeZone.Contains(timeZone))
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value);
        }
    }
}