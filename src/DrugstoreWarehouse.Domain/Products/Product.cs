using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DrugstoreWarehouse.Products
{
    public class Product: FullAuditedEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
    }
}
