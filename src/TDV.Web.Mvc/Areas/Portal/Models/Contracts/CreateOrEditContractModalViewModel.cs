using TDV.Payment.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Contracts
{
    public class CreateOrEditContractModalViewModel
    {
        public CreateOrEditContractDto Contract { get; set; }

        public string RegionName { get; set; }

        public string CompanyDisplayProperty { get; set; }

        public List<ContractRegionLookupTableDto> ContractRegionList { get; set; }

        public List<ContractCompanyLookupTableDto> ContractCompanyList { get; set; }

        public bool IsEditMode => Contract.Id.HasValue;
    }
}