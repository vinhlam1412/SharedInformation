using Microsoft.Extensions.DependencyInjection;
using HQSOFT.SharedInformation.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using HQSOFT.eBiz.GeneralLedger;

namespace HQSOFT.SharedInformation.Blazor;

[DependsOn(
    typeof(SharedInformationApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class SharedInformationBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SharedInformationBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SharedInformationBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SharedInformationMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SharedInformationBlazorModule).Assembly);
        });

        context.Services.AddDevExpressBlazor(options => {
            options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
            options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
        });
        context.Services.AddDevExpressBlazor();
    }
}
