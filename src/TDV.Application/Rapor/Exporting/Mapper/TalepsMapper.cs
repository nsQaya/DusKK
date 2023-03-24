using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Kalite.Dtos;
using TDV.Kalite;
using TDV.Rapor.Dtos;

namespace TDV.Rapor.Exporting.Mapper
{
    public class TalepsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditTalepDto, Talep>().ReverseMap();
            configuration.CreateMap<TalepDto, Talep>().ReverseMap();

            configuration.CreateMap<Talep, GetTalepForViewDto>()
                .ForMember(dest => dest.Talep, opts => opts.MapFrom(src => src));
        }
    }

    
}
