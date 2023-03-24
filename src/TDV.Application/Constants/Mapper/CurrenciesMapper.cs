using Abp.AutoMapper;
using AutoMapper;
using TDV.Constants.Dtos;

namespace TDV.Constants.Mapper
{
    public class CurrenciesMapper
    {
        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CurrencyDto, Currency>().ReverseMap();

            configuration.CreateMap<Currency, GetCurrencyForViewDto>()
                .ForMember(dest => dest.Currency, opts => opts.MapFrom(src => src));
        }
    }
}
