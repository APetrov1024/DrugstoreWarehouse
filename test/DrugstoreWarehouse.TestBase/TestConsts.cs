using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse
{
    public class TestConsts
    {
        public static class InitialData
        {
            public static class Products
            {
                public const int Count = 3;
                public static class Product1
                {
                    public const string Name = "Product 1";
                }

                public static class Product2
                {
                    public const string Name = "Product 2";
                }

                public static class Product3
                {
                    public const string Name = "Product 3";
                }
            }

            public static class Drugstores
            {
                public const int Count = 2;
                public static class Drugstore1
                {
                    public const string Name = "Store 1";
                    public const string Address = "Address 1";
                    public const string TelNumber = "123456";

                }
                public static class Drugstore2
                {
                    public const string Name = "Store 2";
                    public const string Address = "Address 2";
                    public const string TelNumber = "654321";

                }
            }

            public static class Warehouses
            {
                public static int Count = 3;
                public static class Warehouse1
                {
                    public const string Name = "WH 1";
                }
                public static class Warehouse2
                {
                    public const string Name = "WH 2";
                }
                public static class Warehouse3
                {
                    public const string Name = "WH 3";
                }
            }

            public static class Batches
            {
                public const int Count = 5;
                public static class Batch1
                {
                    public const int Quantity = 10;
                }

                public static class Batch2
                {
                    public const int Quantity = 25;
                }
                public static class Batch3
                {
                    public const int Quantity = 20;
                }
                public static class Batch4
                {
                    public const int Quantity = 15;
                }
                public static class Batch5
                {
                    public const int Quantity = 100;
                }
            }
            

        }
    }
}
