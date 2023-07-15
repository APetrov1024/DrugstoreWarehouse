using System.Threading.Tasks;

namespace DrugstoreWarehouse.Data;

public interface IDrugstoreWarehouseDbSchemaMigrator
{
    Task MigrateAsync();
}
