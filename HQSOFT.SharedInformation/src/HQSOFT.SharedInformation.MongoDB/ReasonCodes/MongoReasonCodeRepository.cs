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

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class MongoReasonCodeRepository : MongoDbRepository<SharedInformationMongoDbContext, ReasonCode, Guid>, IReasonCodeRepository
    {
        public MongoReasonCodeRepository(IMongoDbContextProvider<SharedInformationMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ReasonCode>> GetListAsync(
            string filterText = null,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, code, type, description, accountId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ReasonCodeConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<ReasonCode>>()
                .PageBy<ReasonCode, IMongoQueryable<ReasonCode>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string code = null,
           string type = null,
           string description = null,
           Guid? accountId = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, code, type, description, accountId);
            return await query.As<IMongoQueryable<ReasonCode>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ReasonCode> ApplyFilter(
            IQueryable<ReasonCode> query,
            string filterText,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code.Contains(filterText) || e.Type.Contains(filterText) || e.Description.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(type), e => e.Type.Contains(type))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(accountId.HasValue, e => e.AccountId == accountId);
        }
    }
}