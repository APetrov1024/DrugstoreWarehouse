using DrugstoreWarehouse.Drugstores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using DrugstoreWarehouse.Localization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DrugstoreWarehouse.Permissions;

namespace DrugstoreWarehouse.Web.Pages.Drugstores
{
    [Authorize(DrugstoreWarehousePermissions.Drugstores.Edit)]
    public class CreateUpdateDrugstoreModalModel : DrugstoreWarehousePageModel
    {
        private readonly IDrugstoresAppService _drugstoresAppService;

        public CreateUpdateDrugstoreModalModel(
          IDrugstoresAppService drugstoresAppService)
        {
            _drugstoresAppService = drugstoresAppService;
        }

        [BindProperty]
        public CreateUpdateDrugstoreModalVM VM { get; set; }

        public async Task OnGetAsync(Guid? drugstoreId)
        {
            VM = new CreateUpdateDrugstoreModalVM
            {
                DrugstoreId = drugstoreId,
                ModalCaption = drugstoreId.HasValue ? L[LocalizerKeys.ModalCaptions.CreateUpdateDrugstore.Edit] : L[LocalizerKeys.ModalCaptions.CreateUpdateDrugstore.Create],
            };
            if (drugstoreId.HasValue)
            {
                var drugstore = await _drugstoresAppService.GetAsync(drugstoreId.Value);
                VM.Name = drugstore.Name;
                VM.Address = drugstore.Address;
                VM.TelNumber = drugstore.TelNumber;
            }
        }
    }

    public class CreateUpdateDrugstoreModalVM
    {
        [HiddenInput]
        public Guid? DrugstoreId { get; set; }

        public string ModalCaption { get; set; } = string.Empty;

        [Required]
        [MaxLength(DrugstoreConsts.MaxNameLength)]
        [DisplayName(LocalizerKeys.FieldName.Drugstore.Name)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(DrugstoreConsts.MaxAdressLength)]
        [DisplayName(LocalizerKeys.FieldName.Drugstore.Address)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(DrugstoreConsts.MaxTelNumberLength)]
        [DisplayName(LocalizerKeys.FieldName.Drugstore.TelNumber)]
        public string TelNumber { get; set; } = string.Empty;


    }

}
