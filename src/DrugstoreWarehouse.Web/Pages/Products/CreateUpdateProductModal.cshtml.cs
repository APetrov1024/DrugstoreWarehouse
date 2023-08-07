using DrugstoreWarehouse.Localization;
using DrugstoreWarehouse.Permissions;
using DrugstoreWarehouse.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Products
{
    [Authorize(DrugstoreWarehousePermissions.Products.Edit)]
    public class CreateUpdateProductModalModel : DrugstoreWarehousePageModel
    {
        private readonly IProductsAppService _productAppService;
        
        public CreateUpdateProductModalModel(IProductsAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [BindProperty]
        public CreateUpdateProductModalVM VM { get; set; }

        public async Task  OnGetAsync(Guid? productId)
        {
            VM = new CreateUpdateProductModalVM
            {
                ProductId = productId,
                ModalCaption = productId.HasValue ? L[LocalizerKeys.ModalCaptions.CreateUpdateProduct.Edit] : L[LocalizerKeys.ModalCaptions.CreateUpdateProduct.Create],
            };
            if (productId.HasValue)
            { 
                var product = await _productAppService.GetAsync(productId.Value);
                VM.Name = product.Name;
            }
        }
    }

    public class CreateUpdateProductModalVM
    {
        [HiddenInput]
        public Guid? ProductId { get; set; }

        public string ModalCaption { get; set; } = string.Empty;

        [Required]
        [MaxLength(ProductConsts.MaxNameLength)]
        [DisplayName(LocalizerKeys.FieldName.Product.Name)]
        public string Name { get; set; } = string.Empty;
    }
}
