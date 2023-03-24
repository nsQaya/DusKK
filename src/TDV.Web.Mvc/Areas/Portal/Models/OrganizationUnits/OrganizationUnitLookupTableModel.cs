using System.Collections.Generic;
using TDV.Organizations.Dto;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.OrganizationUnits
{
    public class OrganizationUnitLookupTableModel : IOrganizationUnitsEditViewModel
    {
        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }
        
        public List<string> MemberedOrganizationUnits { get; set; }

        public OrganizationUnitLookupTableModel()
        {
            AllOrganizationUnits = new List<OrganizationUnitDto>();
            MemberedOrganizationUnits = new List<string>();
        }
    }
}