using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Batches
{
    public interface IBatchesAppService
    {
        Task<BatchDto> GetAsync(Guid id);
        Task<List<BatchDto>> GetListAsync(Guid warehouseId);
        Task<BatchDto> CreateAsync(CreateUpdateBatchDto dto);
        Task<BatchDto> UpdateAsync(Guid id, CreateUpdateBatchDto dto);
        Task DeleteAsync(Guid id);
    }
}
