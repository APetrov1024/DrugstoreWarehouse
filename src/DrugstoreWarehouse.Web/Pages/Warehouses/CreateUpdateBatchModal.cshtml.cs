using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
using DrugstoreWarehouse.Localization;
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using DrugstoreWarehouse.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace DrugstoreWarehouse.Web.Pages.Warehouses
{
    [Authorize(DrugstoreWarehousePermissions.Warehouses.Edit)]
    public class CreateUpdateBatchModalModel : DrugstoreWarehousePageModel
    {

        private readonly IBatchesAppService _batchesAppService;
        private readonly IWarehousesAppService _warehousesAppService;
        private readonly IProductsAppService _productsAppService;

        [BindProperty]
        public CreateUpdateBatchModalVM VM { get; set; }

        public CreateUpdateBatchModalModel(
            IBatchesAppService batchesAppService,
            IWarehousesAppService warehousesAppService,
            IProductsAppService productsAppService)
        {
            _batchesAppService = batchesAppService;
            _warehousesAppService = warehousesAppService;
            _productsAppService = productsAppService;
        }

        public async Task OnGetAsync(Guid warehouseId, Guid? batchId)
        {
            var products = await _productsAppService.GetListAsync();
            if (products.Count == 0)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.ProductsNotExists]);
            }
            VM = new CreateUpdateBatchModalVM
            {
                BatchId = batchId,
                ModalCaption = batchId.HasValue ? L[LocalizerKeys.ModalCaptions.CreateUpdateBatch.Edit] : L[LocalizerKeys.ModalCaptions.CreateUpdateBatch.Create],
                Quantity = 0,
                Products = products
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                    .ToList(),
                ProductId = products.First().Id,
                WarehouseName = (await _warehousesAppService.GetAsync(warehouseId)).Name,
            };
            if (batchId.HasValue)
            {
                var batch = await _batchesAppService.GetAsync(batchId.Value);
                VM.Quantity = batch.Quantity;
            }
        }
    }

    public class CreateUpdateBatchModalVM
    {
        [HiddenInput]
        public Guid? BatchId { get; set; }

        public string ModalCaption { get; set; } = string.Empty;

        [Required]
        [Range(BatchConsts.MinQuantity, BatchConsts.MaxQuantity)]
        [DisplayName(LocalizerKeys.FieldName.Batch.Quantity)]
        public int Quantity { get; set; }

        [Required]
        [DisplayName(LocalizerKeys.FieldName.Batch.ProductName)]
        public Guid ProductId { get; set; }

        [DisplayName(LocalizerKeys.FieldName.Batch.WarehouseName)]
        public string WarehouseName { get; set; } = string.Empty;
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
    }

}
