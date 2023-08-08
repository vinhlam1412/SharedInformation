using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace HQSOFT.SharedInformation.Seed;

public class SharedInformationHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly SharedInformationSampleDataSeeder _sharedInformationSampleDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public SharedInformationHttpApiHostDataSeedContributor(
        SharedInformationSampleDataSeeder sharedInformationSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _sharedInformationSampleDataSeeder = sharedInformationSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _sharedInformationSampleDataSeeder.SeedAsync(context!);
        }
    }
}
