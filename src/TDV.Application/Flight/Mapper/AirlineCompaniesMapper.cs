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

namespace TDV.Location.Mapper
{
    public class AirlineCompaniesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditAirlineCompanyDto, AirlineCompany>().ReverseMap();
            configuration.CreateMap<AirlineCompanyDto, AirlineCompany>().ReverseMap();
            configuration.CreateMap<AirlineCompany, GetAirlineCompanyForViewDto>()
                .ForMember(dest => dest.AirlineCompany, opts => opts.MapFrom(src => src));
        }
    }
}
