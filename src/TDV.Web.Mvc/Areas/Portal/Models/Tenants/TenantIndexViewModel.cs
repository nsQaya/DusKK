using System.Collections.Generic;
using TDV.Editions.Dto;

namespace TDV.Web.Areas.Portal.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}