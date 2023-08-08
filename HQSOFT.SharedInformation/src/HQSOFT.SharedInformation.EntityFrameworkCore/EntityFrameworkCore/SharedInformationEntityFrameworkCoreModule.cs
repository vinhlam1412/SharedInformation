using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using HQSOFT.SharedInformation.Countries;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using HQSOFT.eBiz.GeneralLedger;
using HQSOFT.eBiz.GeneralLedger.EntityFrameworkCore;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation.EntityFrameworkCore;

[DependsOn(
    typeof(SharedInformationDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(GeneralLedgerEntityFrameworkCoreModule)
)]
public class SharedInformationEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SharedInformationDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<Country, Countries.EfCoreCountryRepository>();

            options.AddRepository<State, States.EfCoreStateRepository>();

            options.AddRepository<Province, Provinces.EfCoreProvinceRepository>();

            options.AddRepository<District, Districts.EfCoreDistrictRepository>();

            options.AddRepository<Ward, Wards.EfCoreWardRepository>();

            options.AddRepository<ReasonCode, ReasonCodes.EfCoreReasonCodeRepository>();

            options.AddRepository<Company, Companies.EfCoreCompanyRepository>();

        });

    }
}