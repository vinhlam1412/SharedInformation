using HQSOFT.SharedInformation.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace HQSOFT.SharedInformation;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(SharedInformationEntityFrameworkCoreTestModule)
    )]
public class SharedInformationDomainTestModule : AbpModule
{

}
