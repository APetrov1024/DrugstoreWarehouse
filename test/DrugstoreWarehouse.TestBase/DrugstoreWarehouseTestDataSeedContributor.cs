using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace DrugstoreWarehouse;

public class DrugstoreWarehouseTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Batch, Guid> _batchesRepository;
    private readonly IRepository<Drugstore, Guid> _drugstoresRepository;
    private readonly IRepository<Product, Guid> _productsRepository;
    private readonly IRepository<Warehouse, Guid> _warehousesRepository;

    public DrugstoreWarehouseTestDataSeedContributor(
        IRepository<Batch, Guid> batchesRepository,
        IRepository<Drugstore, Guid> drugstoresRepository,
        IRepository<Product, Guid> productsRepository,
        IRepository<Warehouse, Guid> warehousesRepository)
    {
        _batchesRepository = batchesRepository;
        _drugstoresRepository = drugstoresRepository;
        _productsRepository = productsRepository;
        _warehousesRepository = warehousesRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */
        var product1 = await NewProductAsync(TestConsts.InitialData.Products.Product1.Name);
        var product2 = await NewProductAsync(TestConsts.InitialData.Products.Product2.Name);
        var product3 = await NewProductAsync(TestConsts.InitialData.Products.Product3.Name);

        var store1 = await NewDrugstoreAsync(
            TestConsts.InitialData.Drugstores.Drugstore1.Name, 
            TestConsts.InitialData.Drugstores.Drugstore1.Address, 
            TestConsts.InitialData.Drugstores.Drugstore1.TelNumber 
            );
        var store2 = await NewDrugstoreAsync(
           TestConsts.InitialData.Drugstores.Drugstore2.Name,
           TestConsts.InitialData.Drugstores.Drugstore2.Address,
           TestConsts.InitialData.Drugstores.Drugstore2.TelNumber
           );

        var wh1 = await NewWarehouseAsync(TestConsts.InitialData.Warehouses.Warehouse1.Name, store1.Id);
        var wh2 = await NewWarehouseAsync(TestConsts.InitialData.Warehouses.Warehouse2.Name, store1.Id);
        var wh3 = await NewWarehouseAsync(TestConsts.InitialData.Warehouses.Warehouse3.Name, store2.Id);

        var batch1 = await NewBatchAsync(product1.Id, wh1.Id, TestConsts.InitialData.Batches.Batch1.Quantity);
        var batch2 = await NewBatchAsync(product1.Id, wh1.Id, TestConsts.InitialData.Batches.Batch2.Quantity);
        var batch3 = await NewBatchAsync(product2.Id, wh1.Id, TestConsts.InitialData.Batches.Batch3.Quantity);
        var batch4 = await NewBatchAsync(product3.Id, wh2.Id, TestConsts.InitialData.Batches.Batch4.Quantity);
        var batch5 = await NewBatchAsync(product1.Id, wh3.Id, TestConsts.InitialData.Batches.Batch5.Quantity);
    }

    private async Task<Product> NewProductAsync(string name)
    {
        var product = new Product { Name = name };
        return await _productsRepository.InsertAsync(product);
    }

    private async Task<Drugstore> NewDrugstoreAsync(string name, string address, string tel)
    {
        var drugstore = new Drugstore { Name = name, Address = address, TelNumber = tel };
        return await _drugstoresRepository.InsertAsync(drugstore);
    }

    private async Task<Warehouse> NewWarehouseAsync(string name, Guid drugstoreId)
    {
        var warehouse = new Warehouse { Name = name, DrugstoreId = drugstoreId };
        return await _warehousesRepository.InsertAsync(warehouse);
    }

    private async Task<Batch> NewBatchAsync(Guid productId, Guid warehouseId, int quantity)
    {
        var batch = new Batch { ProductId = productId, WarehouseId = warehouseId, Quantity = quantity };
        return await _batchesRepository.InsertAsync(batch);
    }

}
