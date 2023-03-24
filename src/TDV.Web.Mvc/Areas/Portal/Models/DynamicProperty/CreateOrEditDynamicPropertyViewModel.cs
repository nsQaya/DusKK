using System.Collections.Generic;
using TDV.DynamicEntityProperties.Dto;

namespace TDV.Web.Areas.Portal.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
