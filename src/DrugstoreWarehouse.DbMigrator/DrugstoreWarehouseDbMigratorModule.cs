using DrugstoreWarehouse.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DrugstoreWarehouse.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DrugstoreWarehouseEntityFrameworkCoreModule),
    typeof(DrugstoreWarehouseApplicationContractsModule)
    )]
public class DrugstoreWarehouseDbMigratorModule : AbpModule
{
}
