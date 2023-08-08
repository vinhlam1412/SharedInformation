using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public interface IReasonCodeRepository : IRepository<ReasonCode, Guid>
    {
        Task<List<ReasonCode>> GetListAsync(
            string filterText = null,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ReasonCodeWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
);

        Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string type = null,
            string description = null,
            Guid? accountId = null,
            CancellationToken cancellationToken = default);
    }
}