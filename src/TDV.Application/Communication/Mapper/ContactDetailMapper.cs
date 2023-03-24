using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Communication.Dtos;
using TDV.Flight.Dtos;
using TDV.Flight;

namespace TDV.Communication.Mapper
{
    public class ContactDetailMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditContactDetailDto, ContactDetail>().ReverseMap();
            configuration.CreateMap<ContactDetailDto, ContactDetail>().ReverseMap();
            configuration.CreateMap<ContactDetail, GetContactDetailForViewDto>()
                .ForMember(dest => dest.ContactDetail, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.ContactIdentifyNo, opts => opts.MapFrom(src => src.ContactFk.IdentifyNo));

        }
    }
}
