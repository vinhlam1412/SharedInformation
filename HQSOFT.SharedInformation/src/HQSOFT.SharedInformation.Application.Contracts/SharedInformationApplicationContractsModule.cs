using HQSOFT.eBiz.GeneralLedger;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation;

[DependsOn(
    typeof(SharedInformationDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(GeneralLedgerApplicationContractsModule)
    )]
public class SharedInformationApplicationContractsModule : AbpModule
{

}
