using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.States
{
    public interface IStateRepository : IRepository<State, Guid>
    {
        Task<List<State>> GetListAsync(
            string filterText = null,
            Guid? countryId = null,
            int? idxMin = null,
            int? idxMax = null,
            string stateCode = null,
            string stateName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? countryId = null,
            int? idxMin = null,
            int? idxMax = null,
            string stateCode = null,
            string stateName = null,
            CancellationToken cancellationToken = default);
    }
}