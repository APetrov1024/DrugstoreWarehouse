using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DrugstoreWarehouse.Data;
using Volo.Abp.DependencyInjection;

namespace DrugstoreWarehouse.EntityFrameworkCore;

public class EntityFrameworkCoreDrugstoreWarehouseDbSchemaMigrator
    : IDrugstoreWarehouseDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreDrugstoreWarehouseDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the DrugstoreWarehouseDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<DrugstoreWarehouseDbContext>()
            .Database
            .MigrateAsync();
    }
}
