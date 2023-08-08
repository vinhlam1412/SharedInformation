using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using HQSOFT.eBiz.GeneralLedger;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(SharedInformationApplicationContractsModule),
    typeof(AbpHttpClientModule),
    typeof(GeneralLedgerHttpApiClientModule)
    )]
public class SharedInformationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(SharedInformationApplicationContractsModule).Assembly,
            SharedInformationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SharedInformationHttpApiClientModule>();
        });
    }
}
