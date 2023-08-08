using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.SharedInformation.Shared;

namespace HQSOFT.SharedInformation.States
{
    public interface IStatesAppService : IApplicationService
    {
        Task<PagedResultDto<StateDto>> GetListAsync(GetStatesInput input);

        Task<StateDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<StateDto> CreateAsync(StateCreateDto input);

        Task<StateDto> UpdateAsync(Guid id, StateUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(StateExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}