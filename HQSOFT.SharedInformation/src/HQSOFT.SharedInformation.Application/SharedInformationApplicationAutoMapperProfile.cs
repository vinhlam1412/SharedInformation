using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using System;
using HQSOFT.SharedInformation.Shared;
using Volo.Abp.AutoMapper;
using HQSOFT.SharedInformation.Countries;
using AutoMapper;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation;

public class SharedInformationApplicationAutoMapperProfile : Profile
{
    public SharedInformationApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Country, CountryDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<Country, CountryExcelDto>();

        CreateMap<State, StateDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<State, StateExcelDto>();
        CreateMap<Country, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Code));

        CreateMap<Province, ProvinceDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<Province, ProvinceExcelDto>();

        CreateMap<District, DistrictDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<District, DistrictExcelDto>();
        CreateMap<Province, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.ProvinceName));

        CreateMap<Ward, WardDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<Ward, WardExcelDto>();
        CreateMap<District, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DistrictName));

        CreateMap<ReasonCode, ReasonCodeDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<ReasonCodeWithNavigationProperties, ReasonCodeWithNavigationPropertiesDto>();
        CreateMap<ReasonCode, ReasonCodeExcelDto>();

        CreateMap<Company, CompanyDto>()
           .Ignore(x => x.IsChanged); ;
        CreateMap<Company, CompanyExcelDto>();
    }
}