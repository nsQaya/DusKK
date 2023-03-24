using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;

namespace TDV.Payment.Mapper
{
    public class ContractsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditContractDto, Contract>().ReverseMap();
            configuration.CreateMap<ContractDto, Contract>().ReverseMap();

            configuration.CreateMap<Contract, GetContractForViewDto>()
                .ForMember(dest => dest.Contract, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.RegionName, opts => opts.MapFrom(src => src.RegionFk.Name));
        }
    }
}
