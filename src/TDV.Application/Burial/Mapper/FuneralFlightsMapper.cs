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
    public class FuneralFlightsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralFlightDto, FuneralFlight>().ReverseMap();
            configuration.CreateMap<FuneralFlightDto, FuneralFlight>().ReverseMap();

            configuration.CreateMap<List<FuneralFlight>, FuneralFlightDto>()
                .ConvertUsing(typeof(ListToSingleConverter<FuneralFlight, FuneralFlightDto>));

            configuration.CreateMap<List<FuneralFlight>, CreateOrEditFuneralFlightDto>()
                .ConvertUsing(typeof(ListToSingleConverter<FuneralFlight, CreateOrEditFuneralFlightDto>));

            configuration.CreateMap<FuneralFlight, GetFuneralFlightForViewDto>()
                .ForMember(dest => dest.AirlineCompanyCode, opts => opts.MapFrom(src => src.AirlineCompanyFk.Code))
                .ForMember(dest => dest.AirportName, opts => opts.MapFrom(src => src.LiftOffAirportFk.Name))
                .ForMember(dest => dest.AirportName2, opts => opts.MapFrom(src => src.LangingAirportFk.Name))
                .ForMember(dest => dest.FuneralName, opts => opts.MapFrom(src => "(" + src.FuneralFk.TransferNo + ") " + src.FuneralFk.Name + " " + src.FuneralFk.Surname))
                .ForMember(dest => dest.FuneralFlight, opts => opts.MapFrom(src => src));
        }
    }
}
