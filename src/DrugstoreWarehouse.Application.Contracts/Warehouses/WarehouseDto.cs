using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DrugstoreWarehouse.Warehouses
{
    public class WarehouseDto: EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid DrugstoreId { get; set; }
    }
}
