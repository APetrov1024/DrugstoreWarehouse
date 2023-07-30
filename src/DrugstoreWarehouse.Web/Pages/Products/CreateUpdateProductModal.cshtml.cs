using DrugstoreWarehouse.Localization;
using DrugstoreWarehouse.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Products
{
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

        [Required]
        [MaxLength(ProductConsts.MaxNameLength)]
        [DisplayName(LocalizerKeys.FieldName.Product.Name)]
        public string Name { get; set; } = string.Empty;
    }
}
