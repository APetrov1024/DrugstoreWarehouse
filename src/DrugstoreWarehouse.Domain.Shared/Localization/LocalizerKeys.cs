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
            public const string ProductsNotExists = Prefix + ":ProductsNotExists";
            public static class EntityNotFound
            {
                public const string Prefix = Errors.Prefix + ":EntityNotFound";
                public const string Product = Prefix + ":Product";
                public const string Warehouse = Prefix + ":Warehouse";
                public const string Batch = Prefix + ":Batch";
                public const string Drugstore = Prefix + ":Drugstore";
            }

            public static class FieldEmpty
            {
                public const string Prefix = Errors.Prefix + ":FieldEmpty";
                public const string Name = Prefix + ":Name";
                public const string Quantity_Int_Positive = Prefix + ":Quantity_Int_Positive";
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

            public static class Warehouse
            {
                public const string Prefix = FieldName.Prefix + ":Warehouse";
                public const string Name = Prefix + ":Name";
                public const string DrugstoreName = Prefix + ":DrugstoreName";
            }

            public static class Batch
            {
                public const string Prefix = FieldName.Prefix + ":Batch";
                public const string ProductName = Prefix + ":ProductName";
                public const string Quantity = Prefix + ":Quantity";
                public const string WarehouseName = Prefix + ":WarehouseName";
            }

            public static class Drugstore
            {
                public const string Prefix = FieldName.Prefix + ":Drugstore";
                public const string Name = Prefix + ":Name";
                public const string Address = Prefix + ":Address";
                public const string TelNumber = Prefix + ":TelNumber";
                public const string ProductName = Prefix + ":ProductName";
                public const string Quantity = Prefix + ":Quantity";
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

            public static class Batch
            {
                public const string Prefix = Message.Prefix + ":Batch";
                public const string DeleteConfirmHeader = Prefix + ":DeleteConfirmHeader";
                public const string DeleteConfirmMessage = Prefix + ":DeleteConfirmMessage";
                public const string SelectWarehouse = Prefix + ":SelectWarehouse";
            }

            public static class Drugstore
            {
                public const string Prefix = Message.Prefix + ":Drugstore";
                public const string DeleteConfirmHeader = Prefix + ":DeleteConfirmHeader";
                public const string DeleteConfirmMessage = Prefix + ":DeleteConfirmMessage";
                public const string SelectDrugstore = Prefix + ":SelectDrugstore";
            }
        }

        public static class ModalCaptions
        {
            public const string Prefix = "ModalCaptions";

            public static class CreateUpdateBatch
            {
                public const string Prefix = ModalCaptions.Prefix + ":CreateUpdateBatch";
                public const string Create = Prefix + ":Create";
                public const string Edit = Prefix + ":Edit";
            }

            public static class CreateUpdateWarehouse
            {
                public const string Prefix = ModalCaptions.Prefix + ":CreateUpdateWarehouse";
                public const string Create = Prefix + ":Create";
                public const string Edit = Prefix + ":Edit";
            }

            public static class CreateUpdateDrugstore
            {
                public const string Prefix = ModalCaptions.Prefix + ":CreateUpdateDrugstore";
                public const string Create = Prefix + ":Create";
                public const string Edit = Prefix + ":Edit";
            }

            public static class CreateUpdateProduct
            {
                public const string Prefix = ModalCaptions.Prefix + ":CreateUpdateProduct";
                public const string Create = Prefix + ":Create";
                public const string Edit = Prefix + ":Edit";
            }
        }

        public static class Buttons
        {
            public const string Prefix = "Buttons";

            public const string Add = Prefix + ":Add";
            public const string AddWarehouse = Prefix + ":AddWarehouse";
            public const string AddBatch = Prefix + ":AddBatch";
        }
    }
}
