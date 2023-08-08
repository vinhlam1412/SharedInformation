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
using HQSOFT.SharedInformation.States;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.States
{

    [Authorize(SharedInformationPermissions.States.Default)]
    public class StatesAppService : ApplicationService, IStatesAppService
    {
        private readonly IDistributedCache<StateExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IStateRepository _stateRepository;
        private readonly StateManager _stateManager;

        public StatesAppService(IStateRepository stateRepository, StateManager stateManager, IDistributedCache<StateExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _stateRepository = stateRepository;
            _stateManager = stateManager;
        }

        public virtual async Task<PagedResultDto<StateDto>> GetListAsync(GetStatesInput input)
        {
            var totalCount = await _stateRepository.GetCountAsync(input.FilterText, input.CountryId, input.IdxMin, input.IdxMax, input.StateCode, input.StateName);
            var items = await _stateRepository.GetListAsync(input.FilterText, input.CountryId, input.IdxMin, input.IdxMax, input.StateCode, input.StateName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<StateDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<State>, List<StateDto>>(items)
            };
        }

        public virtual async Task<StateDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<State, StateDto>(await _stateRepository.GetAsync(id));
        }

        [Authorize(SharedInformationPermissions.States.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _stateRepository.DeleteAsync(id);
        }

        [Authorize(SharedInformationPermissions.States.Create)]
        public virtual async Task<StateDto> CreateAsync(StateCreateDto input)
        {

            var state = await _stateManager.CreateAsync(
            input.CountryId, input.Idx, input.StateCode, input.StateName
            );

            return ObjectMapper.Map<State, StateDto>(state);
        }

        [Authorize(SharedInformationPermissions.States.Edit)]
        public virtual async Task<StateDto> UpdateAsync(Guid id, StateUpdateDto input)
        {

            var state = await _stateManager.UpdateAsync(
            id,
            input.CountryId, input.Idx, input.StateCode, input.StateName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<State, StateDto>(state);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(StateExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _stateRepository.GetListAsync(input.FilterText, input.CountryId, input.IdxMin, input.IdxMax, input.StateCode, input.StateName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<State>, List<StateExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "States.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new StateExcelDownloadTokenCacheItem { Token = token },
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