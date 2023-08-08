using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace HQSOFT.SharedInformation.EntityFrameworkCore;

public class SharedInformationHttpApiHostMigrationsDbContext : AbpDbContext<SharedInformationHttpApiHostMigrationsDbContext>
{
    public SharedInformationHttpApiHostMigrationsDbContext(DbContextOptions<SharedInformationHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureSharedInformation();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
    }
}
