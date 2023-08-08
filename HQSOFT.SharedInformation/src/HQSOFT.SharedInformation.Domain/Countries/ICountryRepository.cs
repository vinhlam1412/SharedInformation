using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.Countries
{
    public interface ICountryRepository : IRepository<Country, Guid>
    {
        Task<List<Country>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string code = null,
            string description = null,
            string dateFormat = null,
            string timeFormat = null,
            string timeZone = null,
            int? idxMin = null,
            int? idxMax = null,
            CancellationToken cancellationToken = default);
    }
}