using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrugstoreWarehouse.Drugstores
{
    public class CreateUpdateDrugstoreDto
    {
        [Required]
        [MaxLength(DrugstoreConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(DrugstoreConsts.MaxAdressLength)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(DrugstoreConsts.MaxTelNumberLength)]
        public string TelNumber { get; set; } = string.Empty;
    }
}
