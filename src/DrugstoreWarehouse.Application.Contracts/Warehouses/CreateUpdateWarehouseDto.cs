using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrugstoreWarehouse.Warehouses
{
    public class CreateUpdateWarehouseDto
    {
        [Required]
        [MaxLength(WarehouseConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        public Guid DrugstoreId { get; set; }
    }
}
