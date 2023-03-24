using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TDV.Burial.Dtos
{
    public class GetFuneralFlightForEditOutput
    {
        public CreateOrEditFuneralFlightDto FuneralFlight { get; set; }

        public string FuneralName { get; set; }

        public string AirlineCompanyCode { get; set; }

        public string AirportName { get; set; }

        public string AirportName2 { get; set; }

        public List<AirlineCompanyLookupTableDto> AirlineCompanyList { get; set; }
        public List<AirportLookupTableDto> AirportList{ get; set; }

    }
}