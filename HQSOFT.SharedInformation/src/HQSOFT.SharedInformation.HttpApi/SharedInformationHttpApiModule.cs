using Localization.Resources.AbpUi;
using HQSOFT.SharedInformation.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using HQSOFT.eBiz.GeneralLedger;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(SharedInformationApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(GeneralLedgerHttpApiModule)
    )]
public class SharedInformationHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(SharedInformationHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SharedInformationResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
