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
    public class FuneralDocumentsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralDocumentDto, FuneralDocument>().ReverseMap();
            configuration.CreateMap<FuneralDocumentDto, FuneralDocument>().ReverseMap();


            configuration.CreateMap<FuneralDocument, GetFuneralDocumentForViewDto>()
                .ForMember(dest => dest.FuneralDocument, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.FuneralDisplayProperty, opts => opts.MapFrom(src => "("+src.FuneralFk.TransferNo +") "+ src.FuneralFk.Name +" "+ src.FuneralFk.Surname));
        }
    }
}
