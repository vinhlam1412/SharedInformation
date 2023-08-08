using HQSOFT.SharedInformation.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace HQSOFT.SharedInformation.Companies
{
    public interface ICompaniesAppService : IApplicationService
    {
        Task<PagedResultDto<CompanyDto>> GetListAsync(GetCompaniesInput input);

        //Task<CompanyWithNavigationPropertiesDto> GetListNavigationAsync(GetCompaniesInput input);

        Task<CompanyDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<CompanyDto> CreateAsync(CompanyCreateDto input);

        Task<CompanyDto> UpdateAsync(Guid id, CompanyUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CompanyExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}