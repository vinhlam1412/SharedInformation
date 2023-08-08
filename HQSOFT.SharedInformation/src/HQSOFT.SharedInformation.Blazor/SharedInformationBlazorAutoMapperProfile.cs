using Volo.Abp.AutoMapper;
using HQSOFT.SharedInformation.ReasonCodes;
using AutoMapper;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation.Blazor;

public class SharedInformationBlazorAutoMapperProfile : Profile
{
    public SharedInformationBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CountryDto, CountryUpdateDto>();
        CreateMap<CountryDto, CountryCreateDto>();

        CreateMap<StateDto, StateUpdateDto>();
        CreateMap<StateDto, StateCreateDto>();

        CreateMap<ProvinceDto, ProvinceUpdateDto>();
        CreateMap<ProvinceDto, ProvinceCreateDto>();

        CreateMap<DistrictDto, DistrictUpdateDto>();
        CreateMap<DistrictDto, DistrictCreateDto>();

        CreateMap<WardDto, WardUpdateDto>();
        CreateMap<WardDto, WardCreateDto>();

        CreateMap<ReasonCodeDto, ReasonCodeUpdateDto>();
        CreateMap<ReasonCodeDto, ReasonCodeCreateDto>();

        CreateMap<CompanyDto, CompanyCreateDto>();
        CreateMap<CompanyDto, CompanyUpdateDto>();
    }
}