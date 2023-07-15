using System;
using System.Collections.Generic;
using System.Text;
using DrugstoreWarehouse.Localization;
using Volo.Abp.Application.Services;

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
