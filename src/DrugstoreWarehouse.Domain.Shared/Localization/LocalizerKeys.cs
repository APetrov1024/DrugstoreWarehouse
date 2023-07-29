using System;
using System.Collections.Generic;
using System.Text;

namespace DrugstoreWarehouse.Localization
{
    public static class LocalizerKeys
    {
        public static class Errors
        {
            public const string Prefix = "Errors";
            public const string ProductNotFound = Prefix + ":ProductNotFound";
            public const string WarehouseNotFound = Prefix + ":WarhouseNotFound";
            public const string BatchNotFound = Prefix + ":BatchNotFound";
            public const string DrugstoreNotFound = Prefix + ":DrugstoreNotFound";
        }
        public static class Menu
        {
            public const string Prefix = "Menu";
            public const string Drugstores = Prefix + ":Drugstores";
            public const string Products = Prefix + ":Products";
            public const string Warehouses = Prefix + ":Warehouses";

        }


    }
}
