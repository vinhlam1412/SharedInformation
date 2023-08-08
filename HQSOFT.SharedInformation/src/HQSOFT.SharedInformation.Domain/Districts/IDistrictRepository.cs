using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.Districts
{
    public interface IDistrictRepository : IRepository<District, Guid>
    {
        Task<List<District>> GetListAsync(
            string filterText = null,
            Guid? provinceId = null,
            int? idxMin = null,
            int? idxMax = null,
            string districtName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? provinceId = null,
            int? idxMin = null,
            int? idxMax = null,
            string districtName = null,
            CancellationToken cancellationToken = default);
    }
}