using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DrugstoreWarehouse.Batches
{
    public class BatchDto: EntityDto<Guid>
    {
        public int Quantity { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
    }
}
