using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using DrugstoreWarehouse.Warehouses;
using DrugstoreWarehouse.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using DrugstoreWarehouse.Drugstores;
using System.Linq;
using Volo.Abp;

namespace DrugstoreWarehouse.Web.Pages.Warehouses
{
    public class CreateUpdateWarehouseModalModel : DrugstoreWarehousePageModel
    {
        private readonly IWarehousesAppService _warehousesAppService;
        private readonly IDrugstoresAppService _drugstoresAppService;

        public CreateUpdateWarehouseModalModel(
            IWarehousesAppService warehousesAppService,
            IDrugstoresAppService drugstoresAppService)
        {
            _warehousesAppService = warehousesAppService;
            _drugstoresAppService = drugstoresAppService;
        }

        [BindProperty]
        public CreateUpdateWarehouseModalVM VM { get; set; }

        public async Task OnGetAsync(Guid? warehouseId)
        {
            var drugstores = await _drugstoresAppService.GetListAsync();
            if (drugstores.Count == 0) {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.DrugstoresNotExists]);
            }
            VM = new CreateUpdateWarehouseModalVM
            {
                WarehouseId = warehouseId,
                Drugstores = drugstores
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                    .ToList(),
                DrugstoreId = drugstores.First().Id,
            };
            if (warehouseId.HasValue)
            {
                var product = await _warehousesAppService.GetAsync(warehouseId.Value);
                VM.Name = product.Name;
                VM.DrugstoreId = product.DrugstoreId;
            }
        }
    }

    public class CreateUpdateWarehouseModalVM
    {
        [HiddenInput]
        public Guid? WarehouseId { get; set; }

        [Required]
        [MaxLength(WarehouseConsts.MaxNameLength)]
        [DisplayName(LocalizerKeys.FieldName.Product.Name)]
        public string Name { get; set; } = string.Empty;

        
        public Guid DrugstoreId { get; set; }
        public List<SelectListItem> Drugstores { get; set; } = new List<SelectListItem>();
    }
}
