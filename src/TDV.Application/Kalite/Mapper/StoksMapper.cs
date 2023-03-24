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
    public class StoksMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditStokDto, Stok>().ReverseMap();
            configuration.CreateMap<StokDto, Stok>().ReverseMap();

            configuration.CreateMap<Stok, GetStokForViewDto>()
                .ForMember(dest => dest.Stok, opts => opts.MapFrom(src => src));
        }
    }

   
}
