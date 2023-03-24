using TDV.Payment.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.ContractFormules
{
    public class CreateOrEditContractFormuleModalViewModel
    {
        public CreateOrEditContractFormuleDto ContractFormule { get; set; }

        public bool IsEditMode => ContractFormule.Id.HasValue;
    }
}