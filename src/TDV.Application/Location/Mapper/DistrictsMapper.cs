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
    public class DistrictsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditDistrictDto, District>()
                .ReverseMap()
                .ForMember(dest => dest.CountryId, opts => opts.MapFrom(src => src.CityFk.CountryId));
            configuration.CreateMap<DistrictDto, District>().ReverseMap();
            configuration.CreateMap<District, GetDistrictForViewDto>()
                .ForMember(dest => dest.District, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CityDisplayProperty, opts => opts.MapFrom(src => src.CityFk.Code + " " + src.CityFk.Name))
                .ForMember(dest => dest.RegionName, opts => opts.MapFrom(src => src.RegionFk.Name));
        }
    }
}
