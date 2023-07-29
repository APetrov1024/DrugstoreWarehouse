using DrugstoreWarehouse.Batches;
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

namespace DrugstoreWarehouse.Drugstores
{
    public class DrugstoresAppService_tests : DrugstoreWarehouseApplicationTestBase
    {
        private readonly IDrugstoresAppService _drugstoreAppService;
        private readonly IRepository<Drugstore, Guid> _drugstoresRepository;
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Batch, Guid> _batchesRepository;

        public DrugstoresAppService_tests()
        {
            _drugstoreAppService = GetRequiredService<IDrugstoresAppService>();
            _drugstoresRepository = GetRequiredService<IRepository<Drugstore, Guid>>();
            _warehousesRepository = GetRequiredService<IRepository<Warehouse, Guid>>();
            _batchesRepository = GetRequiredService<IRepository<Batch, Guid>>();
        }

        [Fact]
        public async Task Should_Get_All_Drugstores()
        {
            //act
            var drugstores = await _drugstoreAppService.GetListAsync();

            //assert
            drugstores.Count.ShouldBe(TestConsts.InitialData.Drugstores.Count);
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Drugstore1()
        {
            //act
            var drugstores = await _drugstoreAppService.GetListAsync();

            //assert
            drugstores.ShouldContain(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
        }

        [Fact]
        public async Task Should_Get_Product1_From_Initial_Data()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);

            //act
            var drugstore = await _drugstoreAppService.GetAsync(drugstore1.Id);

            //assert 
            drugstore.ShouldNotBeNull();
            drugstore.Id.ShouldBe(drugstore1.Id);
            drugstore.Name.ShouldBe(TestConsts.InitialData.Drugstores.Drugstore1.Name);
        }

        [Fact]
        public async Task Should_Throw_On_Get_Not_Exists()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _drugstoreAppService.GetAsync(Guid.Empty);
            });
        }

        [Fact]
        public async Task Should_Create_New_Drugstore()
        {
            //init
            var dto = new CreateUpdateDrugstoreDto { Name = "New Store", Address = "Address", TelNumber = "111111" };

            //act
            var newProduct = await _drugstoreAppService.CreateAsync(dto);

            //assert
            newProduct.ShouldNotBeNull();
            newProduct.Id.ShouldNotBe(Guid.Empty);
            newProduct.Name.ShouldBe(dto.Name);
            newProduct.Address.ShouldBe(dto.Address);
            newProduct.TelNumber.ShouldBe(dto.TelNumber);
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Empty_Name()
        {

            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _drugstoreAppService.CreateAsync(new CreateUpdateDrugstoreDto { Name = string.Empty });
            });
        }

        [Fact]
        public async Task Should_Update_Drugstore()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
            var dto = new CreateUpdateDrugstoreDto { Name = "Updated", Address = "Updated Address", TelNumber = "000" };

            //act
            var updatedDrugstore = await _drugstoreAppService.UpdateAsync(drugstore1.Id, dto);

            //assert
            updatedDrugstore.ShouldNotBeNull();
            updatedDrugstore.Id.ShouldBe(drugstore1.Id);
            updatedDrugstore.Name.ShouldBe(dto.Name);
            updatedDrugstore.Address.ShouldBe(dto.Address);
            updatedDrugstore.TelNumber.ShouldBe(dto.TelNumber);
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Empty_Name()
        {
            //init
            var dto = new CreateUpdateDrugstoreDto { Name = string.Empty, Address = "Address", TelNumber = "12312" };
            //assert
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
                await _drugstoreAppService.UpdateAsync(drugstore1.Id, dto);
            });
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Empty_Address()
        {
            //init
            var dto = new CreateUpdateDrugstoreDto { Name = "Store", Address = string.Empty, TelNumber = "12312" };
            //assert
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
                await _drugstoreAppService.UpdateAsync(drugstore1.Id, dto);
            });
        }

        [Fact]
        public async Task Should_Delete_Drugstore()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);

            //act
            await _drugstoreAppService.DeleteAsync(drugstore1.Id);
            var drugstore = await _drugstoresRepository.FindAsync(drugstore1.Id);

            //assert
            drugstore.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Delete_Warehouses_And_Batches()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);

            //act
            await _drugstoreAppService.DeleteAsync(drugstore1.Id);
            var warehouses = await _warehousesRepository.GetListAsync(x => x.DrugstoreId == drugstore1.Id);
            var batches = await _batchesRepository.GetListAsync();

            //assert
            warehouses.Count.ShouldBe(0);
            batches.Count.ShouldBe(1);
        }


        [Fact]
        public async Task Should_Agregate_Products()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
            var quantity = TestConsts.InitialData.Batches.Batch1.Quantity;
            quantity += TestConsts.InitialData.Batches.Batch2.Quantity;
            quantity += TestConsts.InitialData.Batches.Batch3.Quantity;
            quantity += TestConsts.InitialData.Batches.Batch4.Quantity;

            //act
            var products = await _drugstoreAppService.GetProductsAsync(drugstore1.Id);

            //assert
            products.Count.ShouldBe(3);
            products.Sum(x => x.Quantity).ShouldBe(quantity);
        }

    }
}
