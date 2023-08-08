using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public interface IReasonCodesAppService : IApplicationService
    {
        Task<PagedResultDto<ReasonCodeDto>> GetListAsync(GetReasonCodesInput input);
        Task<PagedResultDto<ReasonCodeWithNavigationPropertiesDto>> GetListWithNavigationPropertiesAsync(GetReasonCodesInput input);

        Task<ReasonCodeDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ReasonCodeDto> CreateAsync(ReasonCodeCreateDto input);

        Task<ReasonCodeDto> UpdateAsync(Guid id, ReasonCodeUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ReasonCodeExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}