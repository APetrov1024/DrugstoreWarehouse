namespace DrugstoreWarehouse.Permissions;

public static class DrugstoreWarehousePermissions
{
    public const string GroupName = "DrugstoreWarehouse";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Drugstores
    {
        public const string GroupName = DrugstoreWarehousePermissions.GroupName + ".Drugstores";
        public const string View = GroupName + ".View";
        public const string Edit = GroupName + ".Edit";
    }

    public static class Products
    {
        public const string GroupName = DrugstoreWarehousePermissions.GroupName + ".Products";
        public const string View = GroupName + ".View";
        public const string Edit = GroupName + ".Edit";
    }

    public static class Warehouses
    {
        public const string GroupName = DrugstoreWarehousePermissions.GroupName + ".Warehouses";
        public const string View = GroupName + ".View";
        public const string Edit = GroupName + ".Edit";
    }

}
