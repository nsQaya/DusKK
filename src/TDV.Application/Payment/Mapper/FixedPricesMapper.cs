using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;

namespace TDV.Payment.Mapper
{
    public class FixedPricesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFixedPriceDto, FixedPrice>().ReverseMap();
            configuration.CreateMap<FixedPriceDto, FixedPrice>().ReverseMap();

            configuration.CreateMap<FixedPrice, GetFixedPriceForViewDto>()
                .ForMember(dest => dest.FixedPrice, opts => opts.MapFrom(src => src));
        }
    }
}
