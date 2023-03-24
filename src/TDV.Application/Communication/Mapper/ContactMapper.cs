using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Communication.Dtos;

namespace TDV.Communication.Mapper
{
    public class ContactMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditContactDto, Contact>().ReverseMap();
            configuration.CreateMap<ContactDto, Contact>().ReverseMap();
            configuration.CreateMap<Contact, GetContactForViewDto>()
                .ForMember(dest => dest.Contact, opts => opts.MapFrom(src => src));
        }
    }
}
