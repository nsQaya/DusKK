using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;
using TDV.Payment;
using TDV.Kalite.Dtos;

namespace TDV.Kalite.Mapper
{
    public class OlcumsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditOlcumDto, Olcum>().ReverseMap();
            configuration.CreateMap<OlcumDto, Olcum>().ReverseMap();

            configuration.CreateMap<Olcum, GetOlcumForViewDto>()
                .ForMember(dest => dest.Olcum, opts => opts.MapFrom(src => src));
        }
    }

   
}
