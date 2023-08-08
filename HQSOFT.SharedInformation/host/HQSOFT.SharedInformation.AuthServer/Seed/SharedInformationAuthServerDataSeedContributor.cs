using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace HQSOFT.SharedInformation.Seed;

public class SharedInformationAuthServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly SharedInformationSampleIdentityDataSeeder _sharedInformationSampleIdentityDataSeeder;
    private readonly SharedInformationAuthServerDataSeeder _sharedInformationAuthServerDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public SharedInformationAuthServerDataSeedContributor(
        SharedInformationAuthServerDataSeeder sharedInformationAuthServerDataSeeder,
        SharedInformationSampleIdentityDataSeeder sharedInformationSampleIdentityDataSeeder,
        ICurrentTenant currentTenant)
    {
        _sharedInformationAuthServerDataSeeder = sharedInformationAuthServerDataSeeder;
        _sharedInformationSampleIdentityDataSeeder = sharedInformationSampleIdentityDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _sharedInformationSampleIdentityDataSeeder.SeedAsync(context!);
            await _sharedInformationAuthServerDataSeeder.SeedAsync(context!);
        }
    }
}
