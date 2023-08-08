using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using HQSOFT.eBiz.GeneralLedger;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(SharedInformationDomainModule),
    typeof(SharedInformationApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(GeneralLedgerApplicationModule)
    )]
public class SharedInformationApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SharedInformationApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<SharedInformationApplicationModule>(validate: true);
        });
    }
}
