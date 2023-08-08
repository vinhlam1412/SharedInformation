using HQSOFT.eBiz.GeneralLedger;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation.Blazor.WebAssembly;

[DependsOn(
    typeof(SharedInformationBlazorModule),
    typeof(SharedInformationHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
)]
public class SharedInformationBlazorWebAssemblyModule : AbpModule
{

}
