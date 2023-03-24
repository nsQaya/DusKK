using TDV.Kalite.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Olcums
{
    public class CreateOrEditOlcumModalViewModel
    {
        public CreateOrEditOlcumDto Olcum { get; set; }

        public bool IsEditMode => Olcum.Id.HasValue;
    }
}