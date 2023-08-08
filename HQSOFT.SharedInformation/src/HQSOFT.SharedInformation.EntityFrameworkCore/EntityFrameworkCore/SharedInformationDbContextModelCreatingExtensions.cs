using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Wards;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using HQSOFT.eBiz.GeneralLedger.Accounts;
using Volo.Abp.EntityFrameworkCore.Modeling;
using HQSOFT.SharedInformation.Countries;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using HQSOFT.SharedInformation.Companies;

namespace HQSOFT.SharedInformation.EntityFrameworkCore;

public static class SharedInformationDbContextModelCreatingExtensions
{
    public static void ConfigureSharedInformation(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Questions", SharedInformationDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<District>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Districts", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.HasOne<Province>().WithMany().IsRequired().HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.SetNull);
    b.Property(x => x.Idx).HasColumnName(nameof(District.Idx));
    b.HasIndex(x => x.DistrictName).IsUnique();
    b.Property(x => x.DistrictName).HasColumnName(nameof(District.DistrictName)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<State>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "States", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.HasOne<Country>().WithMany().IsRequired().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.SetNull);
    b.Property(x => x.Idx).HasColumnName(nameof(State.Idx));
    b.HasIndex(x => x.StateCode).IsUnique();
    b.Property(x => x.StateCode).HasColumnName(nameof(State.StateCode)).IsRequired();
    b.Property(x => x.StateName).HasColumnName(nameof(State.StateName)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Ward>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Wards", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.HasOne<District>().WithMany().IsRequired().HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.SetNull);
    b.Property(x => x.Idx).HasColumnName(nameof(Ward.Idx));
    b.HasIndex(x => x.WardName).IsUnique();
    b.Property(x => x.WardName).HasColumnName(nameof(Ward.WardName)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Province>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Provinces", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Idx).HasColumnName(nameof(Province.Idx));
    b.HasOne<Country>().WithMany().IsRequired().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.SetNull);
    b.HasIndex(x => x.ProvinceCode).IsUnique();
    b.Property(x => x.ProvinceCode).HasColumnName(nameof(Province.ProvinceCode)).IsRequired();
    b.Property(x => x.ProvinceName).HasColumnName(nameof(Province.ProvinceName)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<ReasonCode>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "ReasonCodes", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.HasIndex(x => x.Code).IsUnique();
    b.Property(x => x.Code).HasColumnName(nameof(ReasonCode.Code)).IsRequired();
    b.Property(x => x.Type).HasColumnName(nameof(ReasonCode.Type)).IsRequired();
    b.Property(x => x.Description).HasColumnName(nameof(ReasonCode.Description)).IsRequired();
    b.Property(x => x.AccountId).HasColumnName(nameof(ReasonCode.AccountId)).IsRequired();
    //b.HasOne<Account>().WithMany().IsRequired().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.SetNull);
});

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Company>(b =>
            {
                b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Companies", SharedInformationDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Abbreviation).HasColumnName(nameof(Company.Abbreviation)).IsRequired();
                b.Property(x => x.CompanyName).HasColumnName(nameof(Company.CompanyName)).IsRequired();
                b.Property(x => x.DefaultCurrency).HasColumnName(nameof(Company.DefaultCurrency)).IsRequired();
                b.Property(x => x.TaxID).HasColumnName(nameof(Company.TaxID)).IsRequired();
                b.Property(x => x.CountryId).HasColumnName(nameof(Company.CountryId));
                b.Property(x => x.IsGroup).HasColumnName(nameof(Company.IsGroup));
                b.Property(x => x.ParentCompany).HasColumnName(nameof(Company.ParentCompany));
                b.Property(x => x.Address1).HasColumnName(nameof(Company.Address1));
                b.Property(x => x.Address2).HasColumnName(nameof(Company.Address2));
                b.Property(x => x.Email).HasColumnName(nameof(Company.Email));
                b.Property(x => x.Web).HasColumnName(nameof(Company.Web));
                b.Property(x => x.Phone1).HasColumnName(nameof(Company.Phone1));
                b.Property(x => x.Phone2).HasColumnName(nameof(Company.Phone2));
                b.Property(x => x.StateId).HasColumnName(nameof(Company.StateId));
                b.Property(x => x.ProvinceId).HasColumnName(nameof(Company.ProvinceId));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Country>(b =>
{
    b.ToTable(SharedInformationDbProperties.DbTablePrefix + "Countries", SharedInformationDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Code).HasColumnName(nameof(Country.Code)).IsRequired();
    b.Property(x => x.Description).HasColumnName(nameof(Country.Description)).IsRequired();
    b.Property(x => x.DateFormat).HasColumnName(nameof(Country.DateFormat));
    b.Property(x => x.TimeFormat).HasColumnName(nameof(Country.TimeFormat));
    b.Property(x => x.TimeZone).HasColumnName(nameof(Country.TimeZone));
    b.Property(x => x.Idx).HasColumnName(nameof(Country.Idx));
});

        }
    }
}