using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;

namespace TDV.Payment.Mapper
{
    public class FixedPriceDetailsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFixedPriceDetailDto, FixedPriceDetail>().ReverseMap();
            configuration.CreateMap<FixedPriceDetailDto, FixedPriceDetail>().ReverseMap();

            configuration.CreateMap<FixedPriceDetail, GetFixedPriceDetailForViewDto>()
                .ForMember(dest => dest.FixedPriceDetail, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.FixedPriceName, opts => opts.MapFrom(src => src.FixedPriceFk.Name));
        }
    }
}
