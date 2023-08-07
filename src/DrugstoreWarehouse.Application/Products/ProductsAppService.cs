using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Localization;
using DrugstoreWarehouse.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace DrugstoreWarehouse.Products
{
    [Authorize]
    public class ProductsAppService: DrugstoreWarehouseAppService, IProductsAppService
    {
        private readonly IRepository<Product, Guid> _productsRepository;
        private readonly IRepository<Batch, Guid> _batchesRepository;

        public ProductsAppService(
            IRepository<Product, Guid> productsRepository,
            IRepository<Batch, Guid> batchesRepository)
        {
            _productsRepository = productsRepository;
            _batchesRepository = batchesRepository;
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            try
            {
                var product = await _productsRepository.GetAsync(id);
                return ObjectMapper.Map<Product, ProductDto>(product);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Product], innerException: ex);
            }
        }

        public async Task<List<ProductDto>> GetListAsync()
        {
            var products = (await _productsRepository.GetListAsync()).OrderBy(x => x.Name).ToList();
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
        }

        [Authorize(DrugstoreWarehousePermissions.Products.Edit)]
        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto dto)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(dto);
            product = await _productsRepository.InsertAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(DrugstoreWarehousePermissions.Products.Edit)]
        public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto dto)
        {
            try
            {
                var product = await _productsRepository.GetAsync(id);
                ObjectMapper.Map(dto, product);
                await _productsRepository.UpdateAsync(product);
                return ObjectMapper.Map<Product, ProductDto>(product);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.EntityNotFound.Product], innerException: ex);
            }
        }

        [Authorize(DrugstoreWarehousePermissions.Products.Edit)]
        public async Task DeleteAsync(Guid id)
        {
            var query = (await _productsRepository.WithDetailsAsync(x => x.Batches))
                .Where(x => x.Id == id);
            var product = await AsyncExecuter.SingleOrDefaultAsync(query);
            if (product != null)
            {
                await _batchesRepository.DeleteManyAsync(product.Batches);
                await _productsRepository.DeleteAsync(product);
            }
        }

    }
}
