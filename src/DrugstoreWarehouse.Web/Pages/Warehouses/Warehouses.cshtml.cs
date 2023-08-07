using DrugstoreWarehouse.Permissions;
using DrugstoreWarehouse.Warehouses;
using DrugstoreWarehouse.Web.Pages.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Warehouses
{
    [Authorize(DrugstoreWarehousePermissions.Warehouses.View)]
    public class WarehousesModel : DrugstoreWarehousePageModel
    {
        private readonly IWarehousesAppService _warehouseAppService;

        public WarehousesModel(IWarehousesAppService warehouseAppService)
        {
            _warehouseAppService = warehouseAppService;
        }

        [HiddenInput]
        public bool ReadOnly { get; set; }
        public List<WarehouseListItemVM> Warehouses { get; set; } = new List<WarehouseListItemVM>();

        public async Task OnGetAsync()
        {
            ReadOnly = !(await AuthorizationService.IsGrantedAsync(DrugstoreWarehousePermissions.Drugstores.Edit));
            Warehouses = (await _warehouseAppService.GetListAsync())
                .Select(x => new WarehouseListItemVM { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }

    public class WarehouseListItemVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
