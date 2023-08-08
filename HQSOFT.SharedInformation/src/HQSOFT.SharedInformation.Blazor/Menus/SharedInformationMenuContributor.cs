using Microsoft.AspNetCore.Authorization;
using HQSOFT.SharedInformation.Localization;
using HQSOFT.SharedInformation.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace HQSOFT.SharedInformation.Blazor.Menus;

public class SharedInformationMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        //if (context.Menu.Name == StandardMenus.Main)
        //{
        //    await ConfigureMainMenuAsync(context);
        //}

        var moduleMenu = AddModuleMenuItem(context);

        var menuGeographicalSubdivisions = AddMenuGeographicalSubdivisions(context, moduleMenu);

        AddMenuItemStates(context, menuGeographicalSubdivisions);

        AddMenuItemProvinces(context, menuGeographicalSubdivisions);

        AddMenuItemDistricts(context, menuGeographicalSubdivisions);

        AddMenuItemWards(context, menuGeographicalSubdivisions);

        AddMenuItemReasonCodes(context, moduleMenu);

        AddMenuItemCompanies(context, moduleMenu);

        AddMenuItemCountries(context, moduleMenu);
    }
    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            SharedInformationMenus.Prefix,
            context.GetLocalizer<SharedInformationResource>()["Menu:SharedInformation"],
            icon: "fa fa-share-alt"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }

    private static ApplicationMenuItem AddMenuGeographicalSubdivisions(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        var subMenu = new ApplicationMenuItem(
            SharedInformationMenus.GeographicalSubdivisions,
            context.GetLocalizer<SharedInformationResource>()["Menu:GeographicalSubdivisions"],
            icon: "fa fa-globe"
        );

        parentMenu.Items.AddIfNotContains(subMenu);
        return subMenu;
    }

    private static void AddMenuItemStates(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.States,
                context.GetLocalizer<SharedInformationResource>()["Menu:States"],
                "/SharedInformation/GeographicalSubdivisions/States",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.States.Default
            )
        );
    }

    private static void AddMenuItemProvinces(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.Provinces,
                context.GetLocalizer<SharedInformationResource>()["Menu:Provinces"],
                "/SharedInformation/GeographicalSubdivisions/Provinces",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.Provinces.Default
            )
        );
    }

    private static void AddMenuItemDistricts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.Districts,
                context.GetLocalizer<SharedInformationResource>()["Menu:Districts"],
                "/SharedInformation/GeographicalSubdivisions/Districts",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.Districts.Default
            )
        );
    }

    private static void AddMenuItemWards(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.Wards,
                context.GetLocalizer<SharedInformationResource>()["Menu:Wards"],
                "/SharedInformation/GeographicalSubdivisions/Wards",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.Wards.Default
            )
        );
    }

    private static void AddMenuItemReasonCodes(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.ReasonCodes,
                context.GetLocalizer<SharedInformationResource>()["Menu:ReasonCodes"],
                "/SharedInformation/ReasonCodes",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.ReasonCodes.Default
            )
        );
    }

    private static void AddMenuItemCompanies(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.Companies,
                context.GetLocalizer<SharedInformationResource>()["Menu:Companies"],
                "/SharedInformation/Companies",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.Companies.Default
            )
        );
    }

    private static void AddMenuItemCountries(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.SharedInformationMenus.Countries,
                context.GetLocalizer<SharedInformationResource>()["Menu:Countries"],
                "/SharedInformation/Countries",
                icon: "fa fa-file-alt",
                requiredPermissionName: SharedInformationPermissions.Countries.Default
            )
        );
    }
}