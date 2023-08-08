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
using HQSOFT.SharedInformation.Wards;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Wards
{

    [Authorize(SharedInformationPermissions.Wards.Default)]
    public class WardsAppService : ApplicationService, IWardsAppService
    {
        private readonly IDistributedCache<WardExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IWardRepository _wardRepository;
        private readonly WardManager _wardManager;

        public WardsAppService(IWardRepository wardRepository, WardManager wardManager, IDistributedCache<WardExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _wardRepository = wardRepository;
            _wardManager = wardManager;
        }

        public virtual async Task<PagedResultDto<WardDto>> GetListAsync(GetWardsInput input)
        {
            var totalCount = await _wardRepository.GetCountAsync(input.FilterText, input.DistrictId, input.IdxMin, input.IdxMax, input.WardName);
            var items = await _wardRepository.GetListAsync(input.FilterText, input.DistrictId, input.IdxMin, input.IdxMax, input.WardName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<WardDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Ward>, List<WardDto>>(items)
            };
        }

        public virtual async Task<WardDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Ward, WardDto>(await _wardRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.Wards.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _wardRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.Wards.Create)]
        public virtual async Task<WardDto> CreateAsync(WardCreateDto input)
        {

            var ward = await _wardManager.CreateAsync(
            input.DistrictId, input.Idx, input.WardName
            );

            return ObjectMapper.Map<Ward, WardDto>(ward);
        }

        [Authorize(SharedInformationPermissions.Wards.Edit)]
        public virtual async Task<WardDto> UpdateAsync(Guid id, WardUpdateDto input)
        {

            var ward = await _wardManager.UpdateAsync(
            id,
            input.DistrictId, input.Idx, input.WardName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Ward, WardDto>(ward);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(WardExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _wardRepository.GetListAsync(input.FilterText, input.DistrictId, input.IdxMin, input.IdxMax, input.WardName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Ward>, List<WardExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Wards.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new WardExcelDownloadTokenCacheItem { Token = token },
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