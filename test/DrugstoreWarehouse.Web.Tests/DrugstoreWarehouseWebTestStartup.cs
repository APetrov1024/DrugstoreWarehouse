using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace DrugstoreWarehouse;

public class DrugstoreWarehouseWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<DrugstoreWarehouseWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
