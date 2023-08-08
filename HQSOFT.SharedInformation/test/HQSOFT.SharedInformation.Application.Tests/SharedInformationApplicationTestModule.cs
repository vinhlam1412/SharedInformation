using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(SharedInformationApplicationModule),
    typeof(SharedInformationDomainTestModule)
    )]
public class SharedInformationApplicationTestModule : AbpModule
{

}
