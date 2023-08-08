using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.Provinces
{
    public interface IProvinceRepository : IRepository<Province, Guid>
    {
        Task<List<Province>> GetListAsync(
            string filterText = null,
            int? idxMin = null,
            int? idxMax = null,
            Guid? countryId = null,
            string provinceCode = null,
            string provinceName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            int? idxMin = null,
            int? idxMax = null,
            Guid? countryId = null,
            string provinceCode = null,
            string provinceName = null,
            CancellationToken cancellationToken = default);
    }
}