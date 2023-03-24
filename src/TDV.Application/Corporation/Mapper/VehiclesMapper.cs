using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Corporation.Dtos;

namespace TDV.Corporation.Mapper
{
    public class VehiclesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditVehicleDto, Vehicle>().ReverseMap();
            configuration.CreateMap<VehicleDto, Vehicle>().ReverseMap();

            configuration.CreateMap<Vehicle, GetVehicleForViewDto>()
                .ForMember(dest => dest.Vehicle, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CompanyDisplayProperty, opts => opts.MapFrom(src => src.CompanyFk.OrganizationUnitFk.DisplayName));
        }
    }
}
