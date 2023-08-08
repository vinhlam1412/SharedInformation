using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.SharedInformation.Districts;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Districts
{
    [RemoteService(Name = "SharedInformation")]
    [Area("sharedInformation")]
    [ControllerName("District")]
    [Route("api/shared-information/districts")]
    public class DistrictController : AbpController, IDistrictsAppService
    {
        private readonly IDistrictsAppService _districtsAppService;

        public DistrictController(IDistrictsAppService districtsAppService)
        {
            _districtsAppService = districtsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<DistrictDto>> GetListAsync(GetDistrictsInput input)
        {
            return _districtsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<DistrictDto> GetAsync(Guid id)
        {
            return _districtsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<DistrictDto> CreateAsync(DistrictCreateDto input)
        {
            return _districtsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<DistrictDto> UpdateAsync(Guid id, DistrictUpdateDto input)
        {
            return _districtsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _districtsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistrictExcelDownloadDto input)
        {
            return _districtsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _districtsAppService.GetDownloadTokenAsync();
        }
    }
}