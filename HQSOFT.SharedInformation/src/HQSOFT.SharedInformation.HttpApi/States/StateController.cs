using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.SharedInformation.States;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.States
{
    [RemoteService(Name = "SharedInformation")]
    [Area("sharedInformation")]
    [ControllerName("State")]
    [Route("api/shared-information/states")]
    public class StateController : AbpController, IStatesAppService
    {
        private readonly IStatesAppService _statesAppService;

        public StateController(IStatesAppService statesAppService)
        {
            _statesAppService = statesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<StateDto>> GetListAsync(GetStatesInput input)
        {
            return _statesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<StateDto> GetAsync(Guid id)
        {
            return _statesAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<StateDto> CreateAsync(StateCreateDto input)
        {
            return _statesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<StateDto> UpdateAsync(Guid id, StateUpdateDto input)
        {
            return _statesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _statesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(StateExcelDownloadDto input)
        {
            return _statesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _statesAppService.GetDownloadTokenAsync();
        }
    }
}