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
using HQSOFT.eBiz.GeneralLedger.Accounts;
using HQSOFT.eBiz.GeneralLedger.EntityFrameworkCore;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class EfCoreReasonCodeRepository : EfCoreRepository<SharedInformationDbContext, ReasonCode, Guid>, IReasonCodeRepository
    {
        private GeneralLedgerDbContext _generalLedgerDbContext;
        public EfCoreReasonCodeRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider, GeneralLedgerDbContext generalLedgerDbContext)
            : base(dbContextProvider)
        {
            _generalLedgerDbContext = generalLedgerDbContext;
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
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, type, description, accountId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ReasonCodeConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<List<ReasonCodeWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, type, description, accountId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ReasonCodeConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ReasonCodeWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from reasonCode in (await GetDbSetAsync())
                   join account in (_generalLedgerDbContext).Set<Account>() on reasonCode.AccountId equals account.Id into accounts
                   from account in accounts.DefaultIfEmpty()
                   select new ReasonCodeWithNavigationProperties
                   {
                       ReasonCode = reasonCode,
                       Account = account
                   };
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, code, type, description, accountId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ReasonCodeWithNavigationProperties> ApplyFilter(
            IQueryable<ReasonCodeWithNavigationProperties> query,
            string filterText,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ReasonCode.Code.ToLower().Contains(filterText.ToLower()) || e.ReasonCode.Type.ToLower().Contains(filterText.ToLower()) || e.ReasonCode.Description.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.ReasonCode.Code.ToLower().Contains(code.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(type), e => e.ReasonCode.Type.ToLower().Contains(type.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.ReasonCode.Description.ToLower().Contains(description.ToLower()))
                    .WhereIf(accountId.HasValue, e => e.ReasonCode.AccountId == accountId);
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
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code.ToLower().Contains(filterText.ToLower()) || e.Type.ToLower().Contains(filterText.ToLower()) || e.Description.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.ToLower().Contains(code.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(type), e => e.Type.ToLower().Contains(type.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.ToLower().Contains(description.ToLower()))
                    .WhereIf(accountId.HasValue, e => e.AccountId == accountId);
        }
    }
}