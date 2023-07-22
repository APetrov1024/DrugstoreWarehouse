using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DrugstoreWarehouse.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace DrugstoreWarehouse;

/* Inherit your application services from this class.
 */
public abstract class DrugstoreWarehouseAppService : ApplicationService
{
    protected DrugstoreWarehouseAppService()
    {
        LocalizationResource = typeof(DrugstoreWarehouseResource);
    }
}
