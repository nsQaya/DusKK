using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Communication.Dtos;

namespace TDV.Communication.Mapper
{
    public class ContactNetsisDetailMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditContactNetsisDetailDto, ContactNetsisDetail>().ReverseMap();
            configuration.CreateMap<ContactNetsisDetailDto, ContactNetsisDetail>().ReverseMap();
            configuration.CreateMap<ContactNetsisDetail, GetContactNetsisDetailForViewDto>()
                .ForMember(dest => dest.ContactNetsisDetail, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.ContactName, opts => opts.MapFrom(src => src.ContactFk.Name));
        }
    }
}
