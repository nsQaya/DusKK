using TDV.Kalite.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Stoks
{
    public class CreateOrEditStokModalViewModel
    {
        public CreateOrEditStokDto Stok { get; set; }

        public bool IsEditMode => Stok.Id.HasValue;
    }
}