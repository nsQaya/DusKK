using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;

namespace TDV.Payment.Mapper
{
    public class ContractFormulesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditContractFormuleDto, ContractFormule>().ReverseMap();
            configuration.CreateMap<ContractFormuleDto, ContractFormule>().ReverseMap();

            configuration.CreateMap<ContractFormule, GetContractFormuleForViewDto>()
                .ForMember(dest => dest.ContractFormule, opts => opts.MapFrom(src => src));
        }
    }
}
