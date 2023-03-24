using TDV.Burial.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using TDV.Location.Dtos;

namespace TDV.Web.Areas.Portal.Models.Funerals
{
    public class DriverAssignmentModel
    {
        public List<VehiclesLookupTableDto> VehicleList { get; set; }
        public List<EmployeeLookupTableDto> EmployeerList { get; set; }

    }
}