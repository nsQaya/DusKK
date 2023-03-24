using TDV.Kalite.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.StokOlcus
{
    public class CreateOrEditStokOlcuModalViewModel
    {
        public CreateOrEditStokOlcuDto StokOlcu { get; set; }

        public string StokAdi { get; set; }

        public string OlcumOlcuTipi { get; set; }

        public List<StokOlcuOlcumLookupTableDto> StokOlcuOlcumList { get; set; }

        public bool IsEditMode => StokOlcu.Id.HasValue;
    }
}