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

namespace HQSOFT.SharedInformation.Districts
{
    public class EfCoreDistrictRepository : EfCoreRepository<SharedInformationDbContext, District, Guid>, IDistrictRepository
    {
        public EfCoreDistrictRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<District>> GetListAsync(
            string filterText = null,
            Guid? provinceId = null,
            int? idxMin = null,
            int? idxMax = null,
            string districtName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, provinceId, idxMin, idxMax, districtName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DistrictConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? provinceId = null,
            int? idxMin = null,
            int? idxMax = null,
            string districtName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, provinceId, idxMin, idxMax, districtName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<District> ApplyFilter(
            IQueryable<District> query,
            string filterText,
            Guid? provinceId = null,
            int? idxMin = null,
            int? idxMax = null,
            string districtName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.DistrictName.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(provinceId.HasValue, e => e.ProvinceId == provinceId)
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(districtName), e => e.DistrictName.ToLower().Contains(districtName.ToLower()));
        }
    }
}