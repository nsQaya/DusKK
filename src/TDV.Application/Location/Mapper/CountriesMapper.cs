using Abp.AutoMapper;
using AutoMapper;
using AutoMapper.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Location.Dtos;

namespace TDV.Location.Mapper
{
    public class CountriesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<Country, GetCountryForViewDto>()
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src));
        }
    }
}
