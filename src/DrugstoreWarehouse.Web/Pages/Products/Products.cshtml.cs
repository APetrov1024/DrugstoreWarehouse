using DrugstoreWarehouse.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Products
{
    [Authorize(DrugstoreWarehousePermissions.Products.View)]
    public class ProductsModel : DrugstoreWarehousePageModel
    {
        [HiddenInput]
        public bool ReadOnly { get; set; }
        public async Task OnGetAsync()
        {
            ReadOnly = !(await AuthorizationService.IsGrantedAsync(DrugstoreWarehousePermissions.Products.Edit));
        }
    }
}
