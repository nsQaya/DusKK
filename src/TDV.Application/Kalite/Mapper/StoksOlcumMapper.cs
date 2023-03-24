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
    public class StoksOlcumMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditStokOlcuDto, StokOlcu>().ReverseMap();
            configuration.CreateMap<StokOlcuDto, StokOlcu>().ReverseMap();

            configuration.CreateMap<StokOlcu, GetStokOlcuForViewDto>()
                .ForMember(dest => dest.StokOlcu, opts => opts.MapFrom(src => src));
        }
    }

   
}
