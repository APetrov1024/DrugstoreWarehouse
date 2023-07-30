using DrugstoreWarehouse.Drugstores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using DrugstoreWarehouse.Localization;
using Volo.Abp.ObjectMapping;
using DrugstoreWarehouse.Batches;
using System.Security.Cryptography.X509Certificates;

namespace DrugstoreWarehouse.Warehouses
{
    public class WarehousesAppService: DrugstoreWarehouseAppService, IWarehousesAppService
    {
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Drugstore, Guid> _drugstoresRepository;
        private readonly IRepository<Batch, Guid> _batchesRepository;

        public WarehousesAppService(
            IRepository<Warehouse, Guid> warehousesRepository,
            IRepository<Drugstore, Guid> drugstoresRepository,
            IRepository<Batch, Guid> batchesRepository)
        {
            _warehousesRepository = warehousesRepository;
            _drugstoresRepository = drugstoresRepository;
            _batchesRepository = batchesRepository;
        }

        public async Task<WarehouseDto> GetAsync(Guid id)
        {
            try
            {
                var warehouse = await _warehousesRepository.GetAsync(id);
                return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Warehouse], innerException: ex);
            }
        }

        public async Task<List<WarehouseDto>> GetListAsync()
        {
            var warehouses = await _warehousesRepository.GetListAsync();
            return ObjectMapper.Map<List<Warehouse>, List<WarehouseDto>>(warehouses);
        }

        private async Task<bool> CheckDrugstoreExists(Guid drugstoreId, bool throwNotFound = true)
        {
            var result = await _drugstoresRepository.AnyAsync(x => x.Id == drugstoreId);
            if (!result && throwNotFound)
            { 
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Drugstore]);
            }
            return result;
        }

        public async Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto dto)
        {
            await CheckDrugstoreExists(dto.DrugstoreId);
            var warehouse = ObjectMapper.Map<CreateUpdateWarehouseDto, Warehouse>(dto);
            warehouse = await _warehousesRepository.InsertAsync(warehouse);
            return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
        }

        public async Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto dto)
        {
            try
            {
                await CheckDrugstoreExists(dto.DrugstoreId);
                var warehouse = await _warehousesRepository.GetAsync(id);
                ObjectMapper.Map(dto, warehouse);
                await _warehousesRepository.UpdateAsync(warehouse);
                return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Drugstore], innerException: ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = (await _warehousesRepository.WithDetailsAsync(x => x.Batches))
                .Where(x => x.Id == id);
            var warehouse = await AsyncExecuter.SingleOrDefaultAsync(query);
            if (warehouse != null)
            {
                await _batchesRepository.DeleteManyAsync(warehouse.Batches);
                await _warehousesRepository.DeleteAsync(warehouse);
            }
        }



    }
}
