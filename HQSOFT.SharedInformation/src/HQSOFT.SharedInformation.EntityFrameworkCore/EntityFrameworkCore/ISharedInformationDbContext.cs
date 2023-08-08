using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using HQSOFT.SharedInformation.Countries;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation.EntityFrameworkCore;

[ConnectionStringName(SharedInformationDbProperties.ConnectionStringName)]
public interface ISharedInformationDbContext : IEfCoreDbContext
{
    DbSet<ReasonCode> ReasonCodes { get; set; }
    DbSet<Ward> Wards { get; set; }
    DbSet<District> Districts { get; set; }
    DbSet<Province> Provinces { get; set; }
    DbSet<State> States { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<Company> Companies { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}