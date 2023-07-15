using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DrugstoreWarehouse.Data;

/* This is used if database provider does't define
 * IDrugstoreWarehouseDbSchemaMigrator implementation.
 */
public class NullDrugstoreWarehouseDbSchemaMigrator : IDrugstoreWarehouseDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
