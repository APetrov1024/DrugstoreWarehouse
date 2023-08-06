using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
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

namespace DrugstoreWarehouse.Batches
{
    public class BatchesAppService_tests : DrugstoreWarehouseApplicationTestBase
    {
        private readonly IBatchesAppService _batchesAppService;
        private readonly IRepository<Batch, Guid> _batchesRepository;
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Product, Guid> _productsRepository;

        public BatchesAppService_tests()
        {
            _batchesAppService = GetRequiredService<IBatchesAppService>();
            _batchesRepository = GetRequiredService<IRepository<Batch, Guid>>();
            _warehousesRepository = GetRequiredService<IRepository<Warehouse, Guid>>();
            _productsRepository = GetRequiredService<IRepository<Product, Guid>>();
        }


        [Fact]
        public async Task Should_Get_All_Batches_On_Warehouse()
        {
            //init 
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            //act
            var batches = await _batchesAppService.GetListAsync(warehouse1.Id);

            //assert
            batches.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Batches_Should_Contain_Product_Info()
        {
            //init 
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            //act
            var batches = await _batchesAppService.GetListAsync(warehouse1.Id);
            var hasNullProduct = batches.Any(x => x.ProductName.IsNullOrWhiteSpace());

            //assert
            hasNullProduct.ShouldBeFalse();
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Batch1()
        {
            //init 
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
            //act
            var batches = await _batchesAppService.GetListAsync(warehouse1.Id);

            //assert
            batches.ShouldContain(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);
        }

        [Fact]
        public async Task Should_Get_Batch_From_Initial_Data()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
            var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);

            //act
            var batch = await _batchesAppService.GetAsync(batch1.Id);

            //assert 
            batch.ShouldNotBeNull();
            batch.Id.ShouldBe(batch1.Id);
            batch.ProductId.ShouldBe(product1.Id);
            batch.WarehouseId.ShouldBe(warehouse1.Id);
            batch.Quantity.ShouldBe(TestConsts.InitialData.Batches.Batch1.Quantity);
        }

        [Fact]
        public async Task Should_Throw_On_Get_Not_Exists()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _batchesAppService.GetAsync(Guid.Empty);
            });
        }

        [Fact]
        public async Task Should_Create_New_Batch()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
            var dto = new CreateUpdateBatchDto { ProductId = product1.Id, WarehouseId = warehouse1.Id, Quantity = 999 };

            //act
            var newBatch = await _batchesAppService.CreateAsync(dto);

            //assert
            newBatch.ShouldNotBeNull();
            newBatch.Id.ShouldNotBe(Guid.Empty);
            newBatch.ProductId.ShouldBe(dto.ProductId);
            newBatch.WarehouseId.ShouldBe(dto.WarehouseId);
            newBatch.Quantity.ShouldBe(dto.Quantity);
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Not_Exists_Product()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = Guid.Empty, WarehouseId = warehouse1.Id, Quantity = 999 });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Not_Exists_Warehouse()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = product1.Id, WarehouseId = Guid.Empty, Quantity = 999 });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Negative_Quantity()
        {
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = product1.Id, WarehouseId = warehouse1.Id, Quantity = -999 });
            });
        }


        [Fact]
        public async Task Should_Update_Batch()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var warehouse2 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse2.Name);
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
            var product2 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product2.Name);
            var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);
            var dto = new CreateUpdateBatchDto { ProductId = product2.Id, WarehouseId = warehouse2.Id, Quantity = 999 };

            //act
            var updatedBatch = await _batchesAppService.UpdateAsync(batch1.Id, dto);

            //assert
            updatedBatch.ShouldNotBeNull();
            updatedBatch.Id.ShouldBe(batch1.Id);
            updatedBatch.ProductId.ShouldBe(dto.ProductId);
            updatedBatch.WarehouseId.ShouldBe(dto.WarehouseId);
            updatedBatch.Quantity.ShouldBe(dto.Quantity);
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Not_Exists_Product()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = Guid.Empty, WarehouseId = warehouse1.Id, Quantity = 999 });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Not_Exists_Warehouse()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = product1.Id, WarehouseId = Guid.Empty, Quantity = 999 });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Negative_Quantity()
        {
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
                var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
                var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);
                await _batchesAppService.CreateAsync(new CreateUpdateBatchDto { ProductId = product1.Id, WarehouseId = warehouse1.Id, Quantity = -999 });
            });
        }

        [Fact]
        public async Task Should_Delete_Batch()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var product1 = await _productsRepository.GetAsync(x => x.Name == TestConsts.InitialData.Products.Product1.Name);
            var batch1 = await _batchesRepository.GetAsync(x => x.WarehouseId == warehouse1.Id && x.ProductId == product1.Id && x.Quantity == TestConsts.InitialData.Batches.Batch1.Quantity);

            //act
            await _batchesAppService.DeleteAsync(batch1.Id);
            var batch = await _warehousesRepository.FindAsync(batch1.Id);

            //assert
            batch.ShouldBeNull();
        }

    }
}
