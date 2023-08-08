using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.Wards
{
    public interface IWardRepository : IRepository<Ward, Guid>
    {
        Task<List<Ward>> GetListAsync(
            string filterText = null,
            Guid? districtId = null,
            int? idxMin = null,
            int? idxMax = null,
            string wardName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? districtId = null,
            int? idxMin = null,
            int? idxMax = null,
            string wardName = null,
            CancellationToken cancellationToken = default);
    }
}