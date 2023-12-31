﻿using System.Threading.Tasks;
using DrugstoreWarehouse.Localization;
using DrugstoreWarehouse.MultiTenancy;
using DrugstoreWarehouse.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace DrugstoreWarehouse.Web.Menus;

public class DrugstoreWarehouseMenuContributor : IMenuContributor
{
    private int _order = 0;

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<DrugstoreWarehouseResource>();

        //context.Menu.Items.Insert(
        //    0,
        //    new ApplicationMenuItem(
        //        DrugstoreWarehouseMenus.Home,
        //        l["Menu:Home"],
        //        "~/",
        //        icon: "fas fa-home",
        //        order: 0
        //    )
        //);

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                DrugstoreWarehouseMenus.Drugstores,
                l[LocalizerKeys.Menu.Drugstores],
                "~/Drugstores/Drugstores",
                icon: "fas fa-clinic-medical",
                order: _order++,
                requiredPermissionName: DrugstoreWarehousePermissions.Drugstores.View
            )
        );
        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                DrugstoreWarehouseMenus.Warehouses,
                l[LocalizerKeys.Menu.Warehouses],
                "~/Warehouses/Warehouses",
                icon: "fas fa-warehouse",
                order: _order++,
                requiredPermissionName: DrugstoreWarehousePermissions.Warehouses.View
            )
        );
        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                DrugstoreWarehouseMenus.Products,
                l[LocalizerKeys.Menu.Products],
                "~/Products/Products",
                icon: "fas fa-pills",
                order: _order++,
                requiredPermissionName: DrugstoreWarehousePermissions.Products.View
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        return Task.CompletedTask;
    }
}
