using DrugstoreWarehouse.Drugstores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugstoreWarehouse.Web.Pages.Drugstores
{
    public class DrugstoresModel : DrugstoreWarehousePageModel
    {
        private readonly IDrugstoresAppService _drugstoresAppService;

        public DrugstoresModel(IDrugstoresAppService drugstoresAppService)
        {
            _drugstoresAppService = drugstoresAppService;
        }

        public List<DrugstoreListItemVM> Drugstores { get; set; } = new List<DrugstoreListItemVM>();

        public async Task OnGetAsync()
        {
            Drugstores = (await _drugstoresAppService.GetListAsync())
                .Select(x => new DrugstoreListItemVM { Id = x.Id, Name = StringifyDrugstore(x) })
                .ToList();
        }

        private string StringifyDrugstore(DrugstoreDto drugstore)
        {
            var sb = new StringBuilder($"{drugstore.Name}");
            if (!drugstore.TelNumber.IsNullOrWhiteSpace() || !drugstore.Address.IsNullOrWhiteSpace())
            {
                sb.Append(" (");
            }
            if (!drugstore.TelNumber.IsNullOrWhiteSpace()) 
            {
                sb.Append($"{drugstore.TelNumber}; ");
            }
            if (!drugstore.Address.IsNullOrWhiteSpace())
            {
                sb.Append($"{drugstore.Address}");
            }
            if (!drugstore.TelNumber.IsNullOrWhiteSpace() || !drugstore.Address.IsNullOrWhiteSpace())
            {
                sb.Append(")");
            }
            return sb.ToString();
        }
    }

    public class DrugstoreListItemVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
