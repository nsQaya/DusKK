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
    public class RegionsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditRegionDto, Region>().ReverseMap();
            configuration.CreateMap<RegionDto, Region>().ReverseMap();
            configuration.CreateMap<Region, GetRegionForViewDto>()
                .ForMember(dest => dest.FixedPriceName, opts => opts.MapFrom(src => src.FixedPriceFk.Name))
                .ForMember(dest => dest.Region, opts => opts.MapFrom(src => src));
        }
    }
}
