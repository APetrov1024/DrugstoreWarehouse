using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DrugstoreWarehouse.Drugstores
{
    public class DrugstoreDto: EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TelNumber { get; set; } = string.Empty;
    }
}
