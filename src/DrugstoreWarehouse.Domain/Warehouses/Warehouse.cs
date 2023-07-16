using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DrugstoreWarehouse.Warehouses
{
    public class Warehouse: FullAuditedEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid DrugstoreId { get; set; }
        public Drugstore? Drugstore { get; set; }
        public List<Batch> Batches { get; set; } = new List<Batch>();

    }
}
