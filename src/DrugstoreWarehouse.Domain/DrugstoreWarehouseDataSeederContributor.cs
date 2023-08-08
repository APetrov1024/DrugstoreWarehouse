using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;

namespace DrugstoreWarehouse
{
    public class DrugstoreWarehouseDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        protected IPermissionDefinitionManager _permissionDefinitionManager;


        public DrugstoreWarehouseDataSeederContributor(
            IPermissionManager permissionManager,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            _permissionManager = permissionManager;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var permissionNames = (await _permissionDefinitionManager.GetPermissionsAsync())
                .Where(x => x.Providers.Count == 0 || x.Providers.Contains(RolePermissionValueProvider.ProviderName))
                .Select(x => x.Name)
                .ToList();
            foreach (var permissionName in permissionNames)
            {
                await _permissionManager.SetForRoleAsync("admin", permissionName, true);
            }
        }
    }
}
