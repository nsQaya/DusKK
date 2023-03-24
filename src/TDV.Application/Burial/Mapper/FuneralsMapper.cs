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
    public class SinlgeToListConverter<T,T1> : ITypeConverter<T, List<T1>>
    {
        public List<T1> Convert(T source, List<T1> destination, ResolutionContext context)
        {
            return new List<T1>() {
                context.Mapper.Map<T1>(source)
            };
        }
    }

    public class ListToSingleConverter<T, T1> : ITypeConverter<List<T>, T1>
    {
        public T1 Convert(List<T> source, T1 destination, ResolutionContext context)
        {
            if (source == null)
            {
                return (T1)Activator.CreateInstance(typeof(T1), null);
            }
            else return context.Mapper.Map<T1>(source.FirstOrDefault());
        }
    }

    public class FuneralsMapper
    {


        public static void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFuneralFlightDto, List<FuneralFlight>>()
                .ConvertUsing(typeof(SinlgeToListConverter<CreateOrEditFuneralFlightDto, FuneralFlight>));

            configuration.CreateMap<CreateOrEditFuneralAddresDto, List<FuneralAddres>>()
                .ConvertUsing(typeof(SinlgeToListConverter<CreateOrEditFuneralAddresDto, FuneralAddres>));

            configuration.CreateMap<CreateOrEditFuneralDto, Funeral>()
                .ReverseMap()
                .ForMember(dest => dest.CountryId, opts => opts.MapFrom(src => src.Addresses.FirstOrDefault().QuarterFk.DistrictFk.CityFk.CountryId))
                .ForMember(dest => dest.CityId, opts => opts.MapFrom(src => src.Addresses.FirstOrDefault().QuarterFk.DistrictFk.CityId))
                .ForMember(dest => dest.DistrictId, opts => opts.MapFrom(src => src.Addresses.FirstOrDefault().QuarterFk.DistrictId))
                .ForMember(dest => dest.QuarterId, opts => opts.MapFrom(src => src.Addresses.FirstOrDefault().QuarterId));

            configuration.CreateMap<FuneralDto, Funeral>()
                .ReverseMap()
                .ForMember(dest => dest.Flight, opts => opts.MapFrom(src => src.Flights))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.Documents, opts => opts.MapFrom(src => src.Documents));

            configuration.CreateMap<Funeral, GetFuneralForViewDto>()
                .ForMember(dest => dest.Funeral, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.FuneralTypeDescription, opts => opts.MapFrom(src => src.TypeFk.Description))
                .ForMember(dest => dest.ContactDisplayProperty, opts => opts.MapFrom(src => src.ContactFk.Name + " " + src.ContactFk.Surname))
                .ForMember(dest => dest.OwnerOrganizationUnitDisplayName, opts => opts.MapFrom(src => src.OwnerOrgUnitFk.DisplayName))
                .ForMember(dest => dest.GiverOrganizationUnitDisplayName, opts => opts.MapFrom(src => src.GiverOrgUnitFk.DisplayName))
                .ForMember(dest => dest.ContractorOrganizationUnitDisplayName, opts => opts.MapFrom(src => src.ContractorOrgUnitFk.DisplayName))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.EmployeePersonFk.FullName))
                .ForMember(dest => dest.FuneralPackageCode, opts => opts.MapFrom(src => src.FuneralPackageFk.Code));

        }
    }
}
