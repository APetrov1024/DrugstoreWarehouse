using DrugstoreWarehouse.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace DrugstoreWarehouse;

[DependsOn(
    typeof(DrugstoreWarehouseEntityFrameworkCoreTestModule)
    )]
public class DrugstoreWarehouseDomainTestModule : AbpModule
{

}
