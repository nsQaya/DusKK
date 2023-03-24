using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Payment.Dtos;

namespace TDV.Payment.Mapper
{
    public class CompanyTransactionsMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCompanyTransactionDto, CompanyTransaction>().ReverseMap();
            configuration.CreateMap<CompanyTransactionDto, CompanyTransaction>().ReverseMap();

            configuration.CreateMap<CompanyTransaction, GetCompanyTransactionForViewDto>()
                .ForMember(dest => dest.CompanyTransaction, opts => opts.MapFrom(src => src));
        }
    }
}
