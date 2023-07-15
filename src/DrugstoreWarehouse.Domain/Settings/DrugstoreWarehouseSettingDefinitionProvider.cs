using Volo.Abp.Settings;

namespace DrugstoreWarehouse.Settings;

public class DrugstoreWarehouseSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(DrugstoreWarehouseSettings.MySetting1));
    }
}
