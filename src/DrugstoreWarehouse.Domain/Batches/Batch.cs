using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DrugstoreWarehouse.Batches
{
    public class Batch: FullAuditedEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public int Quantity { get; set; } 
    }
}
