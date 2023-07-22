using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
using Xunit;

namespace DrugstoreWarehouse.Products
{
    public class ProductsAppService_Tests: DrugstoreWarehouseApplicationTestBase
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IRepository<Product, Guid> _productsRepository;

        public ProductsAppService_Tests()
        {
            _productsAppService = GetRequiredService<IProductsAppService>();
            _productsRepository = GetRequiredService<IRepository<Product, Guid>>();
        }

        [Fact]
        public async Task Should_Get_All_Products()
        {
            //act
            var products = await _productsAppService.GetListAsync();

            //assert
            products.Count.ShouldBe(TestConsts.InitialData.Products.Count);
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Product1()
        {
            //act
            var products = await _productsAppService.GetListAsync();

            //assert
            products.ShouldContain(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
        }

        [Fact]
        public async Task Should_Get_Product1_From_Initial_Data()
        {
            //init
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);

            //act
            var product = await _productsAppService.GetAsync(product1.Id);

            //assert 
            product.ShouldNotBeNull();
            product.Id.ShouldBe(product1.Id);
            product.Name.ShouldBe(TestConsts.InitialData.Products.Product1.Name);
        }

        [Fact]
        public async Task Should_Throw_On_Get_Not_Exists()
        {

            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _productsAppService.GetAsync(Guid.Empty);
            });
        }

        [Fact]
        public async Task Should_Create_New_Product()
        {
            //act
            var newProduct = await _productsAppService.CreateAsync(new CreateUpdateProductDto { Name = "New Product"});

            //assert
            newProduct.ShouldNotBeNull();
            newProduct.Id.ShouldNotBe(Guid.Empty);
            newProduct.Name.ShouldBe("New Product");
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Empty_Name()
        {

            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _productsAppService.CreateAsync(new CreateUpdateProductDto { Name = string.Empty });
            });
        }


        [Fact]
        public async Task Should_Update_Product()
        {
            //init
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);

            //act
            var updatedProduct = await _productsAppService.UpdateAsync(product1.Id, new CreateUpdateProductDto { Name = "Updated Product" });

            //assert
            updatedProduct.ShouldNotBeNull();
            updatedProduct.Id.ShouldBe(product1.Id);
            updatedProduct.Name.ShouldBe("Updated Product");
        }

        [Fact]
        public async Task Should_Throw_On_Update_Not_Exists()
        {

            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _productsAppService.UpdateAsync(Guid.Empty, new CreateUpdateProductDto { Name = "Updated Product" });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Empty_Name()
        {

            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                await _productsAppService.UpdateAsync(product1.Id, new CreateUpdateProductDto { Name = string.Empty });
            });
        }

        [Fact]
        public async Task Should_Delete_Product()
        {
            //init
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);

            //act
            await _productsAppService.DeleteAsync(product1.Id);
            var product = await _productsRepository.FindAsync(product1.Id);

            //assert
            product.ShouldBeNull();
        }


    }
}
