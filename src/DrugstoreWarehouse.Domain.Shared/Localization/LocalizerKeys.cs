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

            public static class EntityNotFound
            {
                public const string Prefix = Errors.Prefix + ":EntityNotFound";
                public const string Product = Prefix + ":Product";
                public const string Warehouse = Prefix + ":Warhouse";
                public const string Batch = Prefix + ":Batch";
                public const string Drugstore = Prefix + ":Drugstore";
            }

            public static class FieldEmpty
            {
                public const string Prefix = Errors.Prefix + ":FieldEmpty";
                public const string Name = Prefix + ":Name";
            }
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
