using DrugstoreWarehouse.Warehouses;
using DrugstoreWarehouse.Web.Pages.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Warehouses
{
    public class WarehousesModel : PageModel
    {
        private readonly IWarehousesAppService _warehouseAppService;

        public WarehousesModel(IWarehousesAppService warehouseAppService)
        {
            _warehouseAppService = warehouseAppService;
        }

        public List<WarehouseListItemVM> Warehouses { get; set; } = new List<WarehouseListItemVM>();

        public async Task OnGetAsync()
        {
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
