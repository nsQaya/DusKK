using Abp.AutoMapper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Flight.Dtos;
using TDV.Flight;
using TDV.Location.Dtos;

namespace TDV.Location.Mapper
{
    public class AirportRegionMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditAirportRegionDto, AirportRegion>().ReverseMap();
            configuration.CreateMap<AirportRegionDto, AirportRegion>().ReverseMap();
        }
    }
}
