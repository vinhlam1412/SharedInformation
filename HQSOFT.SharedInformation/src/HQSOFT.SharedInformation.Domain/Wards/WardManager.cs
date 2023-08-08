using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardManager : DomainService
    {
        private readonly IWardRepository _wardRepository;

        public WardManager(IWardRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        public async Task<Ward> CreateAsync(
        Guid districtId, int idx, string wardName)
        {
            Check.NotNullOrWhiteSpace(wardName, nameof(wardName));

            var ward = new Ward(
             GuidGenerator.Create(),
             districtId, idx, wardName
             );

            return await _wardRepository.InsertAsync(ward);
        }

        public async Task<Ward> UpdateAsync(
            Guid id,
            Guid districtId, int idx, string wardName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(wardName, nameof(wardName));

            var ward = await _wardRepository.GetAsync(id);

            ward.DistrictId = districtId;
            ward.Idx = idx;
            ward.WardName = wardName;

            ward.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _wardRepository.UpdateAsync(ward);
        }

    }
}