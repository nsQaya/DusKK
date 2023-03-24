using Abp.AutoMapper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Location.Dtos;

namespace TDV.Location.Mapper
{
    public class CitiesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCityDto, City>().ReverseMap();
            configuration.CreateMap<CityDto, City>().ReverseMap();
            configuration.CreateMap<City, GetCityForViewDto>()
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CountryDisplayProperty, opts => opts.MapFrom(src=>src.CountryFk.Code+" "+ src.CountryFk.Name));
        }
    }
}
