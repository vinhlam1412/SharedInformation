using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.SharedInformation.Provinces;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Provinces
{
    [RemoteService(Name = "SharedInformation")]
    [Area("sharedInformation")]
    [ControllerName("Province")]
    [Route("api/shared-information/provinces")]
    public class ProvinceController : AbpController, IProvincesAppService
    {
        private readonly IProvincesAppService _provincesAppService;

        public ProvinceController(IProvincesAppService provincesAppService)
        {
            _provincesAppService = provincesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<ProvinceDto>> GetListAsync(GetProvincesInput input)
        {
            return _provincesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<ProvinceDto> GetAsync(Guid id)
        {
            return _provincesAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<ProvinceDto> CreateAsync(ProvinceCreateDto input)
        {
            return _provincesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<ProvinceDto> UpdateAsync(Guid id, ProvinceUpdateDto input)
        {
            return _provincesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _provincesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProvinceExcelDownloadDto input)
        {
            return _provincesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _provincesAppService.GetDownloadTokenAsync();
        }
    }
}