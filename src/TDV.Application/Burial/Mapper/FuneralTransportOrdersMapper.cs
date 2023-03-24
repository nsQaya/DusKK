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
    public class FuneralTransportOrdersMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralTranportOrderDto, FuneralTranportOrder>().ReverseMap();
            configuration.CreateMap<FuneralTranportOrderDto, FuneralTranportOrder>().ReverseMap();

            configuration.CreateMap<FuneralTranportOrder, GetFuneralTranportOrderForViewDto>()
                .ForMember(dest => dest.FuneralTranportOrder, opts => opts.MapFrom(src => src));
        }
    }
}
