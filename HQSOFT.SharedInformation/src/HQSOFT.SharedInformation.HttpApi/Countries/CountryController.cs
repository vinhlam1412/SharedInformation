using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.SharedInformation.Countries;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Countries
{
    [RemoteService(Name = "SharedInformation")]
    [Area("sharedInformation")]
    [ControllerName("Country")]
    [Route("api/shared-information/countries")]
    public class CountryController : AbpController, ICountriesAppService
    {
        private readonly ICountriesAppService _countriesAppService;

        public CountryController(ICountriesAppService countriesAppService)
        {
            _countriesAppService = countriesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<CountryDto>> GetListAsync(GetCountriesInput input)
        {
            return _countriesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<CountryDto> GetAsync(Guid id)
        {
            return _countriesAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<CountryDto> CreateAsync(CountryCreateDto input)
        {
            return _countriesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CountryDto> UpdateAsync(Guid id, CountryUpdateDto input)
        {
            return _countriesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _countriesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(CountryExcelDownloadDto input)
        {
            return _countriesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _countriesAppService.GetDownloadTokenAsync();
        }
    }
}