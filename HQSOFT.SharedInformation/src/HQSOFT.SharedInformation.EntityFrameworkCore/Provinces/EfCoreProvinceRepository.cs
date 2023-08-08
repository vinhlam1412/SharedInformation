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

namespace HQSOFT.SharedInformation.Provinces
{
    public class EfCoreProvinceRepository : EfCoreRepository<SharedInformationDbContext, Province, Guid>, IProvinceRepository
    {
        public EfCoreProvinceRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Province>> GetListAsync(
            string filterText = null,
            int? idxMin = null,
            int? idxMax = null,
            Guid? countryId = null,
            string provinceCode = null,
            string provinceName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, idxMin, idxMax, countryId, provinceCode, provinceName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProvinceConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            int? idxMin = null,
            int? idxMax = null,
            Guid? countryId = null,
            string provinceCode = null,
            string provinceName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, idxMin, idxMax, countryId, provinceCode, provinceName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Province> ApplyFilter(
            IQueryable<Province> query,
            string filterText,
            int? idxMin = null,
            int? idxMax = null,
            Guid? countryId = null,
            string provinceCode = null,
            string provinceName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ProvinceCode.ToLower().Contains(filterText.ToLower()) || e.ProvinceName.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(idxMin.HasValue, e => e.Idx >= idxMin.Value)
                    .WhereIf(idxMax.HasValue, e => e.Idx <= idxMax.Value)
                    .WhereIf(countryId.HasValue, e => e.CountryId == countryId)
                    .WhereIf(!string.IsNullOrWhiteSpace(provinceCode), e => e.ProvinceCode.ToLower().Contains(provinceCode.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(provinceName), e => e.ProvinceName.ToLower().Contains(provinceName.ToLower()));
        }
    }
}