using DrugstoreWarehouse.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DrugstoreWarehouse.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DrugstoreWarehouseController : AbpControllerBase
{
    protected DrugstoreWarehouseController()
    {
        LocalizationResource = typeof(DrugstoreWarehouseResource);
    }
}
