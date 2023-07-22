using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrugstoreWarehouse.Batches
{
    public class CreateUpdateBatchDto
    {
        [Range(BatchConsts.MinQuantity, BatchConsts.MaxQuantity)]
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
    }
}
