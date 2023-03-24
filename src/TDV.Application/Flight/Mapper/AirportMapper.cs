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
    public class AirportMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditAirportDto, Airport>().ReverseMap();
            configuration.CreateMap<AirportDto, Airport>().ReverseMap();
            configuration.CreateMap<Airport, GetAirportForViewDto>()
                .ForMember(dest => dest.Airport, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CityDisplayProperty, opts => opts.MapFrom(src => src.CityFk.Code + " " + src.CityFk.Name))
                .ForMember(dest => dest.CountryDisplayProperty, opts => opts.MapFrom(src => src.CountryFk.Code + " " + src.CountryFk.Name));
        }
    }
}
