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
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Warehouses;

namespace DrugstoreWarehouse.Drugstores
{
    public class DrugstoresAppService: DrugstoreWarehouseAppService, IDrugstoresAppService
    {
        private readonly IRepository<Drugstore, Guid> _drugstoresRepository;
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Batch, Guid> _batchesRepository;

        public DrugstoresAppService(
            IRepository<Drugstore, Guid> drugstoresRepository,
            IRepository<Warehouse, Guid> warehousesRepository,
            IRepository<Batch, Guid> batchesRepository)
        {
            _drugstoresRepository = drugstoresRepository;
            _warehousesRepository = warehousesRepository;
            _batchesRepository = batchesRepository;
        }

        public async Task<DrugstoreDto> GetAsync(Guid id)
        {
            try
            {
                var drugstore = await _drugstoresRepository.GetAsync(id);
                return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.DrugstoreNotFound], innerException: ex);
            }
        }

        public async Task<List<DrugstoreDto>> GetListAsync()
        {
            var drugstores = await _drugstoresRepository.GetListAsync();
            return ObjectMapper.Map<List<Drugstore>, List<DrugstoreDto>>(drugstores);
        }

        public async Task<List<ProductDto>> GetProductsAsync(Guid drugstoreId)
        {
            var query = (await _drugstoresRepository.GetQueryableAsync())
                .Where(x => x.Id == drugstoreId)
                .SelectMany(x => x.Warehouses)
                .SelectMany(x => x.Batches)
                .GroupBy(x => x.Product.Name)
                .Select(g => new ProductDto
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                });
            var result = await AsyncExecuter.ToListAsync(query);
            return result;
        }

        public async Task<DrugstoreDto> CreateAsync(CreateUpdateDrugstoreDto dto)
        {
            var drugstore = ObjectMapper.Map<CreateUpdateDrugstoreDto, Drugstore>(dto);
            drugstore = await _drugstoresRepository.InsertAsync(drugstore);
            return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
        }

        public async Task<DrugstoreDto> UpdateAsync(Guid id, CreateUpdateDrugstoreDto dto)
        {
            try
            {
                var drugstore = await _drugstoresRepository.GetAsync(id);
                ObjectMapper.Map(dto, drugstore);
                await _drugstoresRepository.UpdateAsync(drugstore);
                return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.DrugstoreNotFound], innerException: ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var drugstoreQuery = (await _drugstoresRepository.WithDetailsAsync(x => x.Warehouses))
                .Where(x => x.Id == id);
            var drugstore = await AsyncExecuter.SingleOrDefaultAsync(drugstoreQuery);
            if (drugstore != null)
            { 
                var warehouseIds = drugstore.Warehouses.Select(x => x.Id).Distinct();
                var batches = await _batchesRepository.GetListAsync(x => warehouseIds.Contains(x.WarehouseId));
                await _batchesRepository.DeleteManyAsync(batches);
                await _warehousesRepository.DeleteManyAsync(drugstore.Warehouses);
                await _drugstoresRepository.DeleteAsync(drugstore);
            }
        }


    }
}
