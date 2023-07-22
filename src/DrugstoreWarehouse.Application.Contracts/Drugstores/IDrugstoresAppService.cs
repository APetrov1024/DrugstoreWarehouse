using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Drugstores
{
    public interface IDrugstoresAppService
    {
        Task<DrugstoreDto> GetAsync(Guid id);
        Task<List<DrugstoreDto>> GetListAsync();
        Task<List<ProductDto>> GetProductsAsync(Guid drugstoreId);
        Task<DrugstoreDto> CreateAsync(CreateUpdateDrugstoreDto dto);
        Task<DrugstoreDto> UpdateAsync(Guid id, CreateUpdateDrugstoreDto dto);
        Task DeleteAsync(Guid id);
    }
}
