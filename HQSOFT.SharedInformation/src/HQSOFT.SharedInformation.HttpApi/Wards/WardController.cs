using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.SharedInformation.Wards;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Wards
{
    [RemoteService(Name = "SharedInformation")]
    [Area("sharedInformation")]
    [ControllerName("Ward")]
    [Route("api/shared-information/wards")]
    public class WardController : AbpController, IWardsAppService
    {
        private readonly IWardsAppService _wardsAppService;

        public WardController(IWardsAppService wardsAppService)
        {
            _wardsAppService = wardsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<WardDto>> GetListAsync(GetWardsInput input)
        {
            return _wardsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<WardDto> GetAsync(Guid id)
        {
            return _wardsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<WardDto> CreateAsync(WardCreateDto input)
        {
            return _wardsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<WardDto> UpdateAsync(Guid id, WardUpdateDto input)
        {
            return _wardsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _wardsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(WardExcelDownloadDto input)
        {
            return _wardsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _wardsAppService.GetDownloadTokenAsync();
        }
    }
}