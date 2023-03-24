using System.Collections.Generic;
using TDV.Caching.Dto;

namespace TDV.Web.Areas.Portal.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}