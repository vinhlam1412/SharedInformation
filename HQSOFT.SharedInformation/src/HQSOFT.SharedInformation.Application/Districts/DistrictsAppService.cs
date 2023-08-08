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
using HQSOFT.SharedInformation.Districts;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Districts
{

    [Authorize(SharedInformationPermissions.Districts.Default)]
    public class DistrictsAppService : ApplicationService, IDistrictsAppService
    {
        private readonly IDistributedCache<DistrictExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IDistrictRepository _districtRepository;
        private readonly DistrictManager _districtManager;

        public DistrictsAppService(IDistrictRepository districtRepository, DistrictManager districtManager, IDistributedCache<DistrictExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _districtRepository = districtRepository;
            _districtManager = districtManager;
        }

        public virtual async Task<PagedResultDto<DistrictDto>> GetListAsync(GetDistrictsInput input)
        {
            var totalCount = await _districtRepository.GetCountAsync(input.FilterText, input.ProvinceId, input.IdxMin, input.IdxMax, input.DistrictName);
            var items = await _districtRepository.GetListAsync(input.FilterText, input.ProvinceId, input.IdxMin, input.IdxMax, input.DistrictName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DistrictDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<District>, List<DistrictDto>>(items)
            };
        }

        public virtual async Task<DistrictDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<District, DistrictDto>(await _districtRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.Districts.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _districtRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.Districts.Create)]
        public virtual async Task<DistrictDto> CreateAsync(DistrictCreateDto input)
        {

            var district = await _districtManager.CreateAsync(
            input.ProvinceId, input.Idx, input.DistrictName
            );

            return ObjectMapper.Map<District, DistrictDto>(district);
        }

        [Authorize(SharedInformationPermissions.Districts.Edit)]
        public virtual async Task<DistrictDto> UpdateAsync(Guid id, DistrictUpdateDto input)
        {

            var district = await _districtManager.UpdateAsync(
            id,
            input.ProvinceId, input.Idx, input.DistrictName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<District, DistrictDto>(district);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistrictExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _districtRepository.GetListAsync(input.FilterText, input.ProvinceId, input.IdxMin, input.IdxMax, input.DistrictName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<District>, List<DistrictExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Districts.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new DistrictExcelDownloadTokenCacheItem { Token = token },
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