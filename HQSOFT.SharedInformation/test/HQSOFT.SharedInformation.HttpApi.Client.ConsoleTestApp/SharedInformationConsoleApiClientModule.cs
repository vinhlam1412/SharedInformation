using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SharedInformationHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class SharedInformationConsoleApiClientModule : AbpModule
{

}
