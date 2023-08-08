using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.States
{
    public class StateManager : DomainService
    {
        private readonly IStateRepository _stateRepository;

        public StateManager(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<State> CreateAsync(
        Guid countryId, int idx, string stateCode, string stateName)
        {
            Check.NotNullOrWhiteSpace(stateCode, nameof(stateCode));
            Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

            var state = new State(
             GuidGenerator.Create(),
             countryId, idx, stateCode, stateName
             );

            return await _stateRepository.InsertAsync(state);
        }

        public async Task<State> UpdateAsync(
            Guid id,
            Guid countryId, int idx, string stateCode, string stateName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(stateCode, nameof(stateCode));
            Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

            var state = await _stateRepository.GetAsync(id);

            state.CountryId = countryId;
            state.Idx = idx;
            state.StateCode = stateCode;
            state.StateName = stateName;

            state.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _stateRepository.UpdateAsync(state);
        }

    }
}