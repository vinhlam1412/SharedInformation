using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.Wards
{
    public interface IWardsAppService : IApplicationService
    {
        Task<PagedResultDto<WardDto>> GetListAsync(GetWardsInput input);

        Task<WardDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<WardDto> CreateAsync(WardCreateDto input);

        Task<WardDto> UpdateAsync(Guid id, WardUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(WardExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}