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
using HQSOFT.SharedInformation.Provinces;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Provinces
{

    [Authorize(SharedInformationPermissions.Provinces.Default)]
    public class ProvincesAppService : ApplicationService, IProvincesAppService
    {
        private readonly IDistributedCache<ProvinceExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IProvinceRepository _provinceRepository;
        private readonly ProvinceManager _provinceManager;

        public ProvincesAppService(IProvinceRepository provinceRepository, ProvinceManager provinceManager, IDistributedCache<ProvinceExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _provinceRepository = provinceRepository;
            _provinceManager = provinceManager;
        }

        public virtual async Task<PagedResultDto<ProvinceDto>> GetListAsync(GetProvincesInput input)
        {
            var totalCount = await _provinceRepository.GetCountAsync(input.FilterText, input.IdxMin, input.IdxMax, input.CountryId, input.ProvinceCode, input.ProvinceName);
            var items = await _provinceRepository.GetListAsync(input.FilterText, input.IdxMin, input.IdxMax, input.CountryId, input.ProvinceCode, input.ProvinceName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ProvinceDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Province>, List<ProvinceDto>>(items)
            };
        }

        public virtual async Task<ProvinceDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Province, ProvinceDto>(await _provinceRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.Provinces.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _provinceRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.Provinces.Create)]
        public virtual async Task<ProvinceDto> CreateAsync(ProvinceCreateDto input)
        {

            var province = await _provinceManager.CreateAsync(
            input.Idx, input.CountryId, input.ProvinceCode, input.ProvinceName
            );

            return ObjectMapper.Map<Province, ProvinceDto>(province);
        }

        [Authorize(SharedInformationPermissions.Provinces.Edit)]
        public virtual async Task<ProvinceDto> UpdateAsync(Guid id, ProvinceUpdateDto input)
        {

            var province = await _provinceManager.UpdateAsync(
            id,
            input.Idx, input.CountryId, input.ProvinceCode, input.ProvinceName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Province, ProvinceDto>(province);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProvinceExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _provinceRepository.GetListAsync(input.FilterText, input.IdxMin, input.IdxMax, input.CountryId, input.ProvinceCode, input.ProvinceName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Province>, List<ProvinceExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Provinces.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ProvinceExcelDownloadTokenCacheItem { Token = token },
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