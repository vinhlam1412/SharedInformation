using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using HQSOFT.SharedInformation.Permissions;
using HQSOFT.SharedInformation.Countries;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Countries
{

    [Authorize(SharedInformationPermissions.Countries.Default)]
    public class CountriesAppService : ApplicationService, ICountriesAppService
    {
        private readonly IDistributedCache<CountryExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly ICountryRepository _countryRepository;
        private readonly CountryManager _countryManager;

        public CountriesAppService(ICountryRepository countryRepository, CountryManager countryManager, IDistributedCache<CountryExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _countryRepository = countryRepository;
            _countryManager = countryManager;
        }

        public virtual async Task<PagedResultDto<CountryDto>> GetListAsync(GetCountriesInput input)
        {
            var totalCount = await _countryRepository.GetCountAsync(input.FilterText, input.Code, input.Description, input.DateFormat, input.TimeFormat, input.TimeZone, input.IdxMin, input.IdxMax);
            var items = await _countryRepository.GetListAsync(input.FilterText, input.Code, input.Description, input.DateFormat, input.TimeFormat, input.TimeZone, input.IdxMin, input.IdxMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CountryDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Country>, List<CountryDto>>(items)
            };
        }

        public virtual async Task<CountryDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Country, CountryDto>(await _countryRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.Countries.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _countryRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.Countries.Create)]
        public virtual async Task<CountryDto> CreateAsync(CountryCreateDto input)
        {

            var country = await _countryManager.CreateAsync(
            input.Code, input.Description, input.DateFormat, input.TimeFormat, input.TimeZone, input.Idx
            );

            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        [Authorize(SharedInformationPermissions.Countries.Edit)]
        public virtual async Task<CountryDto> UpdateAsync(Guid id, CountryUpdateDto input)
        {

            var country = await _countryManager.UpdateAsync(
            id,
            input.Code, input.Description, input.DateFormat, input.TimeFormat, input.TimeZone, input.Idx, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CountryExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _countryRepository.GetListAsync(input.FilterText, input.Code, input.Description, input.DateFormat, input.TimeFormat, input.TimeZone, input.IdxMin, input.IdxMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Country>, List<CountryExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Countries.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new CountryExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}