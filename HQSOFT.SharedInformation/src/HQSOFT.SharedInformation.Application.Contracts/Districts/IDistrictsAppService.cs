using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Districts
{
    public interface IDistrictsAppService : IApplicationService
    {
        Task<PagedResultDto<DistrictDto>> GetListAsync(GetDistrictsInput input);

        Task<DistrictDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DistrictDto> CreateAsync(DistrictCreateDto input);

        Task<DistrictDto> UpdateAsync(Guid id, DistrictUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistrictExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}