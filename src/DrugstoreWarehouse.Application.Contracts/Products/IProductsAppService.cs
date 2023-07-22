using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Products
{
    public interface IProductsAppService
    {
        Task<ProductDto> GetAsync(Guid id);
        Task<List<ProductDto>> GetListAsync();
        Task<ProductDto> CreateAsync(CreateUpdateProductDto dto);
        Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto dto);
        Task DeleteAsync(Guid id);
    }
}
