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
using DrugstoreWarehouse.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace DrugstoreWarehouse.Web.Pages.Warehouses
{
    [Authorize(DrugstoreWarehousePermissions.Warehouses.Edit)]
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
                ModalCaption = warehouseId.HasValue ? L[LocalizerKeys.ModalCaptions.CreateUpdateWarehouse.Edit] : L[LocalizerKeys.ModalCaptions.CreateUpdateWarehouse.Create],
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

        public string ModalCaption { get; set; } = string.Empty;

        [Required]
        [MaxLength(WarehouseConsts.MaxNameLength)]
        [DisplayName(LocalizerKeys.FieldName.Warehouse.Name)]
        public string Name { get; set; } = string.Empty;

        
        [DisplayName(LocalizerKeys.FieldName.Warehouse.DrugstoreName)]
        public Guid DrugstoreId { get; set; }
        public List<SelectListItem> Drugstores { get; set; } = new List<SelectListItem>();
    }
}
