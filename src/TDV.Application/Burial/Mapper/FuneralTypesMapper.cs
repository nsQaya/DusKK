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
using TDV.Burial.Dtos;

namespace TDV.Burial.Mapper
{
    public class FuneralTypesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralTypeDto, FuneralType>().ReverseMap();
            configuration.CreateMap<FuneralTypeDto, FuneralType>().ReverseMap();

            configuration.CreateMap<FuneralType, GetFuneralTypeForViewDto>()
                .ForMember(dest => dest.FuneralType, opts => opts.MapFrom(src => src));
        }
    }
}
