using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceManager : DomainService
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceManager(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task<Province> CreateAsync(
        int idx, Guid countryId, string provinceCode, string provinceName)
        {
            Check.NotNullOrWhiteSpace(provinceCode, nameof(provinceCode));
            Check.NotNullOrWhiteSpace(provinceName, nameof(provinceName));

            var province = new Province(
             GuidGenerator.Create(),
             idx, countryId, provinceCode, provinceName
             );

            return await _provinceRepository.InsertAsync(province);
        }

        public async Task<Province> UpdateAsync(
            Guid id,
            int idx, Guid countryId, string provinceCode, string provinceName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(provinceCode, nameof(provinceCode));
            Check.NotNullOrWhiteSpace(provinceName, nameof(provinceName));

            var province = await _provinceRepository.GetAsync(id);

            province.Idx = idx;
            province.CountryId = countryId;
            province.ProvinceCode = provinceCode;
            province.ProvinceName = provinceName;

            province.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _provinceRepository.UpdateAsync(province);
        }

    }
}