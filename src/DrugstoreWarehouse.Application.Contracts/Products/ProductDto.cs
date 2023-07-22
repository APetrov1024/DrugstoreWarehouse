using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DrugstoreWarehouse.Products
{
    public class ProductDto: EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
    }
}
