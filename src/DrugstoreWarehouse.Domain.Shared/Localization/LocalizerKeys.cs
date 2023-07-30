using System;
using System.Collections.Generic;
using System.Text;
using static DrugstoreWarehouse.Localization.LocalizerKeys;

namespace DrugstoreWarehouse.Localization
{
    public static class LocalizerKeys
    {
        public static class Errors
        {
            public const string Prefix = "Errors";

            public const string DrugstoresNotExists = Prefix + ":DrugstoresNotExists";
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

        public static class FieldName
        {
            public const string Prefix = "FieldName";

            public static class Product
            {
                public const string Prefix = FieldName.Prefix + ":Product";
                public const string Name = Prefix + ":Name";
            }
        }

        public static class Message
        {
            public const string Prefix = "Message";

            public static class Common
            {
                public const string Prefix = Message.Prefix + ":Common";
                public const string SuccessfullyDone = Prefix + ":SuccessfullyDone";
                public const string Error = Prefix + ":Error";
            }

            public static class Product
            {
                public const string Prefix = Message.Prefix + ":Product";
                public const string DeleteConfirmHeader = Prefix + ":DeleteConfirmHeader";
                public const string DeleteConfirmMessage = Prefix + ":DeleteConfirmMessage";
            }
        }

    }
}
