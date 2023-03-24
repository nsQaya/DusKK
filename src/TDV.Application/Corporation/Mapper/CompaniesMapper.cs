using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Corporation.Dtos;

namespace TDV.Corporation.Mapper
{
    public class CompaniesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCompanyDto, Company>().ReverseMap();
            configuration.CreateMap<CompanyDto, Company>().ReverseMap();

            configuration.CreateMap<Company, GetCompanyForViewDto>()
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CityDisplayProperty, opts => opts.MapFrom(src => src.CityFk.Name))
                .ForMember(dest => dest.QuarterName, opts => opts.MapFrom(src => src.QuarterFk.Name))
                .ForMember(dest => dest.OrganizationUnitDisplayName, opts => opts.MapFrom(src => src.OrganizationUnitFk.DisplayName));
        }
    }
}
