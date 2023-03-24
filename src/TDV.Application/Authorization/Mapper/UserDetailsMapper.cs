using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Burial.Dtos;
using TDV.Burial.Mapper;
using TDV.Burial;
using TDV.Authorization.Dtos;

namespace TDV.Authorization.Mapper
{
    public class UserDetailsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditUserDetailDto, UserDetail>().ReverseMap();
            configuration.CreateMap<UserDetailDto, UserDetail>().ReverseMap();

            configuration.CreateMap<UserDetail, GetUserDetailForViewDto>()
                .ForMember(dest => dest.UserDetail, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserFk.UserName))
                .ForMember(dest => dest.ContactDisplayProperty, opts => opts.MapFrom(src => src.ContactFk.Name));
        }
       
    }
}
