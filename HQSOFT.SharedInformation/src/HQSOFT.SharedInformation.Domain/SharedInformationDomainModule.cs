using HQSOFT.eBiz.GeneralLedger;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(SharedInformationDomainSharedModule),
    typeof(GeneralLedgerDomainModule)
)]
public class SharedInformationDomainModule : AbpModule
{

}
