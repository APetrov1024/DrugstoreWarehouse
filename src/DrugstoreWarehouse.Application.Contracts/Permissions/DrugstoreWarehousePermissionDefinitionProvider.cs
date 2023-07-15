using DrugstoreWarehouse.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DrugstoreWarehouse.Permissions;

public class DrugstoreWarehousePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DrugstoreWarehousePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DrugstoreWarehousePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DrugstoreWarehouseResource>(name);
    }
}
