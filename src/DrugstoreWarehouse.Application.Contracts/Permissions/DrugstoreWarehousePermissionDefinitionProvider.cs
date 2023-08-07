using DrugstoreWarehouse.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace DrugstoreWarehouse.Permissions;

public class DrugstoreWarehousePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DrugstoreWarehousePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DrugstoreWarehousePermissions.MyPermission1, L("Permission:MyPermission1"));

        context.RemoveGroup(TenantManagementPermissions.GroupName);
        context.RemoveGroup(FeatureManagementPermissions.GroupName);
        context.RemoveGroup(SettingManagementPermissions.GroupName);

        DefineDrugstores(context);
        DefineProducts(context);
        DefineWarehouses(context);
    }

    private void DefineDrugstores(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(DrugstoreWarehousePermissions.Drugstores.GroupName, L(LocalizerKeys.Permissions.Drugstores.Prefix));
        group.AddPermission(DrugstoreWarehousePermissions.Drugstores.View, L(LocalizerKeys.Permissions.Drugstores.View));
        group.AddPermission(DrugstoreWarehousePermissions.Drugstores.Edit, L(LocalizerKeys.Permissions.Drugstores.Edit));
    }

    private void DefineProducts(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(DrugstoreWarehousePermissions.Products.GroupName, L(LocalizerKeys.Permissions.Products.Prefix));
        group.AddPermission(DrugstoreWarehousePermissions.Products.View, L(LocalizerKeys.Permissions.Products.View));
        group.AddPermission(DrugstoreWarehousePermissions.Products.Edit, L(LocalizerKeys.Permissions.Products.Edit));
    }
    private void DefineWarehouses(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(DrugstoreWarehousePermissions.Warehouses.GroupName, L(LocalizerKeys.Permissions.Warehouses.Prefix));
        group.AddPermission(DrugstoreWarehousePermissions.Warehouses.View, L(LocalizerKeys.Permissions.Warehouses.View));
        group.AddPermission(DrugstoreWarehousePermissions.Warehouses.Edit, L(LocalizerKeys.Permissions.Warehouses.Edit));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DrugstoreWarehouseResource>(name);
    }
}
