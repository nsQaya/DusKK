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
    public class FuneralPackagesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralPackageDto, FuneralPackage>().ReverseMap();
            configuration.CreateMap<FuneralPackageDto, FuneralPackage>().ReverseMap();

            configuration.CreateMap<FuneralPackage, GetFuneralPackageForViewDto>()
                 .ForMember(dest => dest.FuneralPackage, opts => opts.MapFrom(src => src));

        }
    }
}
