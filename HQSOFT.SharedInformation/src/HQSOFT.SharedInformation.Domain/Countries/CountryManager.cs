using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryManager : DomainService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryManager(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country> CreateAsync(
        string code, string description, string dateFormat, string timeFormat, string timeZone, int idx)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var country = new Country(
             GuidGenerator.Create(),
             code, description, dateFormat, timeFormat, timeZone, idx
             );

            return await _countryRepository.InsertAsync(country);
        }

        public async Task<Country> UpdateAsync(
            Guid id,
            string code, string description, string dateFormat, string timeFormat, string timeZone, int idx, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var country = await _countryRepository.GetAsync(id);

            country.Code = code;
            country.Description = description;
            country.DateFormat = dateFormat;
            country.TimeFormat = timeFormat;
            country.TimeZone = timeZone;
            country.Idx = idx;

            country.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _countryRepository.UpdateAsync(country);
        }

    }
}