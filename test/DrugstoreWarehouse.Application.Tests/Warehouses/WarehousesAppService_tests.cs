using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
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
using static DrugstoreWarehouse.TestConsts.InitialData.Products;

namespace DrugstoreWarehouse.Warehouses
{
    public  class WarehousesAppService_tests : DrugstoreWarehouseApplicationTestBase
    {
        private readonly IWarehousesAppService _warehousesAppService;
        private readonly IRepository<Warehouse, Guid> _warehousesRepository;
        private readonly IRepository<Drugstore, Guid> _drugstoresRepository;
        private readonly IRepository<Batch, Guid> _batchesRepository;

        public WarehousesAppService_tests()
        {
            _warehousesAppService = GetRequiredService<IWarehousesAppService>();
            _warehousesRepository = GetRequiredService<IRepository<Warehouse, Guid>>();
            _drugstoresRepository = GetRequiredService<IRepository<Drugstore, Guid>>();
            _batchesRepository = GetRequiredService<IRepository<Batch, Guid>>();
        }

        [Fact]
        public async Task Should_Get_All_Warehouses()
        {
            //act
            var warehouses = await _warehousesAppService.GetListAsync();

            //assert
            warehouses.Count.ShouldBe(TestConsts.InitialData.Warehouses.Count);
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Warehouse1()
        {
            //act
            var warehouses = await _warehousesAppService.GetListAsync();

            //assert
            warehouses.ShouldContain(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
        }

        [Fact]
        public async Task Should_Get_Warehouse1_From_Initial_Data()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);

            //act
            var warehouse = await _warehousesAppService.GetAsync(warehouse1.Id);

            //assert 
            warehouse.ShouldNotBeNull();
            warehouse.Id.ShouldBe(warehouse1.Id);
            warehouse.Name.ShouldBe(TestConsts.InitialData.Warehouses.Warehouse1.Name);
        }

        [Fact]
        public async Task Should_Throw_On_Get_Not_Exists()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _warehousesAppService.GetAsync(Guid.Empty);
            });
        }

        [Fact]
        public async Task Should_Create_New_Drugstore()
        {
            //init
            var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
            var dto = new CreateUpdateWarehouseDto { Name = "New WH", DrugstoreId = drugstore1.Id };

            //act
            var newWarehouse = await _warehousesAppService.CreateAsync(dto);

            //assert
            newWarehouse.ShouldNotBeNull();
            newWarehouse.Id.ShouldNotBe(Guid.Empty);
            newWarehouse.Name.ShouldBe(dto.Name);
            newWarehouse.DrugstoreId.ShouldBe(drugstore1.Id);
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Empty_Name()
        {

            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var drugstore1 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore1.Name);
                await _warehousesAppService.CreateAsync(new CreateUpdateWarehouseDto { Name = string.Empty, DrugstoreId = drugstore1.Id });
            });
        }

        [Fact]
        public async Task Should_Throw_On_Create_With_Not_Exists_Drugstore()
        {
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _warehousesAppService.CreateAsync(new CreateUpdateWarehouseDto { Name = "WH", DrugstoreId = Guid.Empty });
            });
        }

        [Fact]
        public async Task Should_Update_Drugstore()
        {
            //init
            var drugstore2 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore2.Name);
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            var dto = new CreateUpdateWarehouseDto { Name = "Updated", DrugstoreId = drugstore2.Id  };

            //act
            var updatedDrugstore = await _warehousesAppService.UpdateAsync(warehouse1.Id, dto);

            //assert
            updatedDrugstore.ShouldNotBeNull();
            updatedDrugstore.Id.ShouldBe(warehouse1.Id);
            updatedDrugstore.Name.ShouldBe(dto.Name);
            updatedDrugstore.DrugstoreId.ShouldBe(dto.DrugstoreId);
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Empty_Name()
        {
            //init
            var drugstore2 = await _drugstoresRepository.GetAsync(x => x.Name == TestConsts.InitialData.Drugstores.Drugstore2.Name);
            var dto = new CreateUpdateWarehouseDto { Name = string.Empty, DrugstoreId = drugstore2.Id };
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            //assert
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _warehousesAppService.UpdateAsync(warehouse1.Id, dto);
            });
        }

        [Fact]
        public async Task Should_Throw_On_Update_With_Not_Exists_Drugstore()
        {
            //init
            var dto = new CreateUpdateWarehouseDto { Name = "Updated", DrugstoreId = Guid.Empty };
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);
            //assert
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _warehousesAppService.UpdateAsync(warehouse1.Id, dto);
            });
        }

        [Fact]
        public async Task Should_Delete_Warehouse()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);

            //act
            await _warehousesRepository.DeleteAsync(warehouse1.Id);
            var warehouse = await _warehousesRepository.FindAsync(warehouse1.Id);

            //assert
            warehouse.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Delete_Batches()
        {
            //init
            var warehouse1 = await _warehousesRepository.GetAsync(x => x.Name == TestConsts.InitialData.Warehouses.Warehouse1.Name);

            //act
            await _warehousesAppService.DeleteAsync(warehouse1.Id);
            var batches = await _batchesRepository.GetListAsync(x => x.WarehouseId == warehouse1.Id);

            //assert
            batches.Count.ShouldBe(0);
        }

    }
}
