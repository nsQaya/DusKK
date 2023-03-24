using TDV.Rapor.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Taleps
{
    public class CreateOrEditTalepModalViewModel
    {
        public CreateOrEditTalepDto Talep { get; set; }

        public string StokAdi { get; set; }

        public List<TalepStokLookupTableDto> TalepStokList { get; set; }

        public bool IsEditMode => Talep.Id.HasValue;
    }
}