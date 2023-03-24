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
    public class FuneralAddressMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralAddresDto, FuneralAddres>().ReverseMap();
            configuration.CreateMap<FuneralAddresDto, FuneralAddres>().ReverseMap();

            configuration.CreateMap<List<FuneralAddres>, FuneralAddresDto>()
                .ConvertUsing(typeof(ListToSingleConverter<FuneralAddres, FuneralAddresDto>));

            configuration.CreateMap<List<FuneralAddres>, CreateOrEditFuneralAddresDto>()
               .ConvertUsing(typeof(ListToSingleConverter<FuneralAddres, CreateOrEditFuneralAddresDto>));

            configuration.CreateMap<FuneralAddres, GetFuneralAddresForViewDto>()
                .ForMember(dest => dest.FuneralAddres, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.QuarterName, opts => opts.MapFrom(src => src.QuarterFk.Name));
        }
    }
}
