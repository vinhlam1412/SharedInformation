using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Provinces
{
    public interface IProvincesAppService : IApplicationService
    {
        Task<PagedResultDto<ProvinceDto>> GetListAsync(GetProvincesInput input);

        Task<ProvinceDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ProvinceDto> CreateAsync(ProvinceCreateDto input);

        Task<ProvinceDto> UpdateAsync(Guid id, ProvinceUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProvinceExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}