using Volo.Abp.Modularity;

namespace DrugstoreWarehouse;

[DependsOn(
    typeof(DrugstoreWarehouseApplicationModule),
    typeof(DrugstoreWarehouseDomainTestModule)
    )]
public class DrugstoreWarehouseApplicationTestModule : AbpModule
{

}
