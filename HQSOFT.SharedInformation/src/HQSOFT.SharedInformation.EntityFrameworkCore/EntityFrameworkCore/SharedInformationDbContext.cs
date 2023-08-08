using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using HQSOFT.SharedInformation.Countries;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation.EntityFrameworkCore;

[ConnectionStringName(SharedInformationDbProperties.ConnectionStringName)]
public class SharedInformationDbContext : AbpDbContext<SharedInformationDbContext>, ISharedInformationDbContext
{
    public DbSet<ReasonCode> ReasonCodes { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Country> Countries { get; set; }

    public DbSet<Company> Companies { get; set; }

    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public SharedInformationDbContext(DbContextOptions<SharedInformationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSharedInformation();
    }
}