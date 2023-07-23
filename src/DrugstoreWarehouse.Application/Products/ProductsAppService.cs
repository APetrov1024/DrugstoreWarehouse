﻿using DrugstoreWarehouse.Localization;
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
    public class ProductsAppService: DrugstoreWarehouseAppService, IProductsAppService
    {
        private readonly IRepository<Product, Guid> _productsRepository;

        public ProductsAppService(IRepository<Product, Guid> productsRepository)
        {
            _productsRepository = productsRepository;
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
                throw new UserFriendlyException(L[LocalizerKeys.Errors.ProductNotFound], innerException: ex);
            }
        }

        public async Task<List<ProductDto>> GetListAsync()
        { 
            var products = await _productsRepository.GetListAsync();
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto dto)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(dto);
            product = await _productsRepository.InsertAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

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
                throw new UserFriendlyException(L[LocalizerKeys.Errors.ProductNotFound], innerException: ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        { 
            await _productsRepository.DeleteAsync(id);
        }

    }
}