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

namespace HQSOFT.SharedInformation.Wards
{
    public class EfCoreWardRepository : EfCoreRepository<SharedInformationDbContext, Ward, Guid>, IWardRepository
    {
        public EfCoreWardRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Ward>> GetListAsync(
            string filterText = null,
            Guid? districtId = null,
            int? idxMin = null,
            int? idxMax = null,
            string wardName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, districtId, idxMin, idxMax, wardName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? WardConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? districtId = null,
            int? idxMin = null,
            int? idxMax = null,
            string wardName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, districtId, idxMin, idxMax, wardName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Ward> ApplyFilter(
            IQueryable<Ward> query,
            string filterText,
            Guid? districtId = null,
            int? idxMin = null,
            int? idxMax = null,
            string wardName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.WardName.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(districtId.HasValue, e => e.DistrictId == districtId)
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(wardName), e => e.WardName.ToLower().Contains(wardName.ToLower()));
        }
    }
}