using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Corporation.Dtos;

namespace TDV.Corporation.Mapper
{
    public class CompanyContactMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCompanyContactDto, CompanyContact>().ReverseMap();
            configuration.CreateMap<CompanyContactDto, CompanyContact>().ReverseMap();

            configuration.CreateMap<CompanyContact, GetCompanyContactForViewDto>()
                .ForMember(dest => dest.CompanyContact, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.CompanyDisplayProperty, opts => opts.MapFrom(src => src.CompanyFk.OrganizationUnitFk.DisplayName))
                .ForMember(dest => dest.ContactName, opts => opts.MapFrom(src => src.ContactFk.Name));
        }
    }
}
