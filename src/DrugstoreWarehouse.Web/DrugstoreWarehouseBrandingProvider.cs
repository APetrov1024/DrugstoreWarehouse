using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace DrugstoreWarehouse.Web;

[Dependency(ReplaceServices = true)]
public class DrugstoreWarehouseBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "DrugstoreWarehouse";
}
