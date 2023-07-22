using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrugstoreWarehouse.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        [MaxLength(ProductConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;
    }
}
