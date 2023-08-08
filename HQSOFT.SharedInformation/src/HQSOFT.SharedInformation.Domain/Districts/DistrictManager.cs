using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictManager : DomainService
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictManager(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<District> CreateAsync(
        Guid provinceId, int idx, string districtName)
        {
            Check.NotNullOrWhiteSpace(districtName, nameof(districtName));

            var district = new District(
             GuidGenerator.Create(),
             provinceId, idx, districtName
             );

            return await _districtRepository.InsertAsync(district);
        }

        public async Task<District> UpdateAsync(
            Guid id,
            Guid provinceId, int idx, string districtName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(districtName, nameof(districtName));

            var district = await _districtRepository.GetAsync(id);

            district.ProvinceId = provinceId;
            district.Idx = idx;
            district.DistrictName = districtName;

            district.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _districtRepository.UpdateAsync(district);
        }

    }
}