using DrugstoreWarehouse.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DrugstoreWarehouse.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class DrugstoreWarehousePageModel : AbpPageModel
{
    protected DrugstoreWarehousePageModel()
    {
        LocalizationResourceType = typeof(DrugstoreWarehouseResource);
    }
}
