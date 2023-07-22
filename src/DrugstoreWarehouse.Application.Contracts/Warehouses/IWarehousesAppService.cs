using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Warehouses
{
    public interface IWarehousesAppService
    {
        Task<WarehouseDto> GetAsync(Guid id);
        Task<List<WarehouseDto>> GetListAsync();
        Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto dto);
        Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto dto);
        Task DeleteAsync(Guid id);
    }
}
