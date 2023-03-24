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
    public class QuartersMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditQuarterDto, Quarter>()
                .ReverseMap()
                .ForMember(dest => dest.CountryId, opts => opts.MapFrom(src => src.DistrictFk.CityFk.CountryId))
                .ForMember(dest => dest.CityId, opts => opts.MapFrom(src => src.DistrictFk.CityId));

            configuration.CreateMap<QuarterDto, Quarter>().ReverseMap();
            configuration.CreateMap<Quarter, GetQuarterForViewDto>()
                .ForMember(dest => dest.Quarter, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.DistrictName, opts => opts.MapFrom(src => src.DistrictFk.Name));
        }
    }
}
