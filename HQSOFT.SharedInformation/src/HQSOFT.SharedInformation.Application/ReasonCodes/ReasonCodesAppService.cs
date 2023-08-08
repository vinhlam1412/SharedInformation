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
using HQSOFT.SharedInformation.ReasonCodes;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.ReasonCodes
{

    [Authorize(SharedInformationPermissions.ReasonCodes.Default)]
    public class ReasonCodesAppService : ApplicationService, IReasonCodesAppService
    {
        private readonly IDistributedCache<ReasonCodeExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IReasonCodeRepository _reasonCodeRepository;
        private readonly ReasonCodeManager _reasonCodeManager;

        public ReasonCodesAppService(IReasonCodeRepository reasonCodeRepository, ReasonCodeManager reasonCodeManager, IDistributedCache<ReasonCodeExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _reasonCodeRepository = reasonCodeRepository;
            _reasonCodeManager = reasonCodeManager;
        }

        public virtual async Task<PagedResultDto<ReasonCodeDto>> GetListAsync(GetReasonCodesInput input)
        {
            var totalCount = await _reasonCodeRepository.GetCountAsync(input.FilterText, input.Code, input.Type, input.Description, input.AccountId);
            var items = await _reasonCodeRepository.GetListAsync(input.FilterText, input.Code, input.Type, input.Description, input.AccountId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ReasonCodeDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ReasonCode>, List<ReasonCodeDto>>(items)
            };
        }

        public virtual async Task<PagedResultDto<ReasonCodeWithNavigationPropertiesDto>> GetListWithNavigationPropertiesAsync(GetReasonCodesInput input)
        {
            var totalCount = await _reasonCodeRepository.GetCountAsync(input.FilterText, input.Code, input.Type, input.Description, input.AccountId);
            var items = await _reasonCodeRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.Type, input.Description, input.AccountId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ReasonCodeWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ReasonCodeWithNavigationProperties>, List<ReasonCodeWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ReasonCodeDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ReasonCode, ReasonCodeDto>(await _reasonCodeRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.ReasonCodes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _reasonCodeRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.ReasonCodes.Create)]
        public virtual async Task<ReasonCodeDto> CreateAsync(ReasonCodeCreateDto input)
        {

            var reasonCode = await _reasonCodeManager.CreateAsync(
            input.Code, input.Type, input.Description, input.AccountId
            );

            return ObjectMapper.Map<ReasonCode, ReasonCodeDto>(reasonCode);
        }

        [Authorize(SharedInformationPermissions.ReasonCodes.Edit)]
        public virtual async Task<ReasonCodeDto> UpdateAsync(Guid id, ReasonCodeUpdateDto input)
        {

            var reasonCode = await _reasonCodeManager.UpdateAsync(
            id,
            input.Code, input.Type, input.Description, input.AccountId, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ReasonCode, ReasonCodeDto>(reasonCode);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ReasonCodeExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _reasonCodeRepository.GetListAsync(input.FilterText, input.Code, input.Type, input.Description, input.AccountId);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<ReasonCode>, List<ReasonCodeExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ReasonCodes.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ReasonCodeExcelDownloadTokenCacheItem { Token = token },
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