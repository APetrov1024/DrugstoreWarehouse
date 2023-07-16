using DrugstoreWarehouse.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DrugstoreWarehouse.Drugstores
{
    public class Drugstore: FullAuditedEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TelNumber { get; set; } = string.Empty;
        public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    }
}
