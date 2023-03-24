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
using TDV.Constants.Dtos;
using TDV.Burial.Dtos;
using TDV.Burial;

namespace TDV.Constants.Mapper
{
    public class DataListMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditDataListDto, DataList>().ReverseMap();
            configuration.CreateMap<DataListDto, DataList>().ReverseMap();

            configuration.CreateMap<DataList, GetDataListForViewDto>()
                .ForMember(dest => dest.DataList, opts => opts.MapFrom(src => src));
        }
    }
}
