using HQSOFT.SharedInformation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HQSOFT.SharedInformation.Permissions;

public class SharedInformationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SharedInformationPermissions.GroupName, L("Permission:SharedInformation"));

        var countryPermission = myGroup.AddPermission(SharedInformationPermissions.Countries.Default, L("Permission:Countries"));
        countryPermission.AddChild(SharedInformationPermissions.Countries.Create, L("Permission:Create"));
        countryPermission.AddChild(SharedInformationPermissions.Countries.Edit, L("Permission:Edit"));
        countryPermission.AddChild(SharedInformationPermissions.Countries.Delete, L("Permission:Delete"));

        var statePermission = myGroup.AddPermission(SharedInformationPermissions.States.Default, L("Permission:States"));
        statePermission.AddChild(SharedInformationPermissions.States.Create, L("Permission:Create"));
        statePermission.AddChild(SharedInformationPermissions.States.Edit, L("Permission:Edit"));
        statePermission.AddChild(SharedInformationPermissions.States.Delete, L("Permission:Delete"));

        var provincePermission = myGroup.AddPermission(SharedInformationPermissions.Provinces.Default, L("Permission:Provinces"));
        provincePermission.AddChild(SharedInformationPermissions.Provinces.Create, L("Permission:Create"));
        provincePermission.AddChild(SharedInformationPermissions.Provinces.Edit, L("Permission:Edit"));
        provincePermission.AddChild(SharedInformationPermissions.Provinces.Delete, L("Permission:Delete"));

        var districtPermission = myGroup.AddPermission(SharedInformationPermissions.Districts.Default, L("Permission:Districts"));
        districtPermission.AddChild(SharedInformationPermissions.Districts.Create, L("Permission:Create"));
        districtPermission.AddChild(SharedInformationPermissions.Districts.Edit, L("Permission:Edit"));
        districtPermission.AddChild(SharedInformationPermissions.Districts.Delete, L("Permission:Delete"));

        var wardPermission = myGroup.AddPermission(SharedInformationPermissions.Wards.Default, L("Permission:Wards"));
        wardPermission.AddChild(SharedInformationPermissions.Wards.Create, L("Permission:Create"));
        wardPermission.AddChild(SharedInformationPermissions.Wards.Edit, L("Permission:Edit"));
        wardPermission.AddChild(SharedInformationPermissions.Wards.Delete, L("Permission:Delete"));

        var reasonCodePermission = myGroup.AddPermission(SharedInformationPermissions.ReasonCodes.Default, L("Permission:ReasonCodes"));
        reasonCodePermission.AddChild(SharedInformationPermissions.ReasonCodes.Create, L("Permission:Create"));
        reasonCodePermission.AddChild(SharedInformationPermissions.ReasonCodes.Edit, L("Permission:Edit"));
        reasonCodePermission.AddChild(SharedInformationPermissions.ReasonCodes.Delete, L("Permission:Delete"));

        var companyPermission = myGroup.AddPermission(SharedInformationPermissions.Companies.Default, L("Permission:Companies"));
        companyPermission.AddChild(SharedInformationPermissions.Companies.Create, L("Permission:Create"));
        companyPermission.AddChild(SharedInformationPermissions.Companies.Edit, L("Permission:Edit"));
        companyPermission.AddChild(SharedInformationPermissions.Companies.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SharedInformationResource>(name);
    }
}