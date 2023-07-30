using AutoMapper.Internal.Mappers;
using DrugstoreWarehouse.Drugstores;
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using DrugstoreWarehouse.Localization;
using System.Web.Http;
using Volo.Abp.ObjectMapping;
using JetBrains.Annotations;

namespace DrugstoreWarehouse.Batches
{
    public class BatchesAppService : DrugstoreWarehouseAppService, IBatchesAppService
    {
        private readonly IRepository<Batch, Guid> _batchesRepository;
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Product, Guid> _productsRepository;

        public BatchesAppService(
            IRepository<Batch, Guid> batchesRepository,
            IRepository<Warehouse, Guid> warehousesRepository,
            IRepository<Product, Guid> productsRepository)
        {
            _batchesRepository = batchesRepository;
            _warehousesRepository = warehousesRepository;
            _productsRepository = productsRepository;
        }

        private async Task<IQueryable<Batch>> GetDetailedBatchQueryAsync()
        {
            return (await _batchesRepository.WithDetailsAsync(
                    x => x.Product
                ));
        }

        private async Task<Batch?> GetDetailedBatchAsync(Guid id, bool throwNotFound = true)
        {
            var query = (await GetDetailedBatchQueryAsync())
                .Where(x => x.Id == id);
            var batch = await AsyncExecuter.SingleOrDefaultAsync(query);
            if (batch == null && throwNotFound)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Batch]);
            }
            return batch;
        }

        [Route("api/app/batches/{batchId}")]
        public async Task<BatchDto> GetAsync(Guid id)
        {
            var batch = await GetDetailedBatchAsync(id);
            return ObjectMapper.Map<Batch, BatchDto>(batch);
        }

        [Route("api/app/batches/list")]
        public async Task<List<BatchDto>> GetListAsync(Guid warehouseId)
        {
            var query = (await GetDetailedBatchQueryAsync())
                .Where(x => x.WarehouseId == warehouseId);
            var batches = await _batchesRepository.GetListAsync(x => x.WarehouseId == warehouseId);
            return ObjectMapper.Map<List<Batch>, List<BatchDto>>(batches);
        }

        private async Task<bool> CheckWarehouseExists(Guid warehouseId, bool throwNotFound = true)
        {
            var result = await _warehousesRepository.AnyAsync(x => x.Id == warehouseId);
            if (!result && throwNotFound)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Warehouse]);
            }
            return result;
        }

        private async Task<bool> CheckProductExists(Guid productId, bool throwNotFound = true)
        {
            var result = await _productsRepository.AnyAsync(x => x.Id == productId);
            if (!result && throwNotFound)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Product]);
            }
            return result;
        }

        public async Task<BatchDto> CreateAsync(CreateUpdateBatchDto dto)
        {
            await CheckProductExists(dto.ProductId);
            await CheckWarehouseExists(dto.WarehouseId);
            var batch = ObjectMapper.Map<CreateUpdateBatchDto, Batch>(dto);
            batch = await _batchesRepository.InsertAsync(batch, autoSave: true);
            batch = await GetDetailedBatchAsync(batch.Id); // чтобы подгрузить зависимые сущности
            return ObjectMapper.Map<Batch, BatchDto>(batch);
        }

        public async Task<BatchDto> UpdateAsync(Guid id, CreateUpdateBatchDto dto)
        {
            try
            {
                await CheckProductExists(dto.ProductId);
                await CheckWarehouseExists(dto.WarehouseId);
                var batch = await GetDetailedBatchAsync(id);
                var isNestedChanged = batch.ProductId != dto.ProductId || batch.WarehouseId != dto.WarehouseId;
                ObjectMapper.Map(dto, batch);
                await _batchesRepository.UpdateAsync(batch);
                batch = await GetDetailedBatchAsync(id); // чтобы обновить зависимые сущности
                return ObjectMapper.Map<Batch, BatchDto>(batch);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Batch], innerException: ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _batchesRepository.DeleteAsync(id);
        }

    }
}
