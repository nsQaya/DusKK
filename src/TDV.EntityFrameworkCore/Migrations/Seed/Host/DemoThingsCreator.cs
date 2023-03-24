using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Corporation;
using TDV.EntityFrameworkCore;
using TDV.Location;
using TDV.Payment;

namespace TDV.Migrations.Seed.Host
{
    public class DemoThingsCreator
    {
        private readonly TDVDbContext _context;

        public DemoThingsCreator(TDVDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var hasCountry= _context.Countries.IgnoreQueryFilters().Any(x => x.Code == "90");
            if(!hasCountry)
            {
                _context.Countries.Add(new()
                {
                    Code = "90",
                    Name = "Türkiye",
                    Order = 1,
                    IsActive = true,

                    Cities = new List<City>
                    {
                        new City
                        {
                            Name= "Ankara",
                            Code= "06",
                            Order=1,
                            IsActive= true,

                            Districts= new List<District>
                            {
                                new District
                                {
                                    Name= "Yenimahalle",

                                    RegionFk= new Region
                                    {
                                      Name= "İç anadolu bölgesi",
                                      Order= 1,
                                      IsActive= true,

                                      FixedPriceFk= new FixedPrice()
                                      {
                                          Name= "İç anadolu bölgesi fiyatlandırma",
                                          Details= new List<FixedPriceDetail>
                                          {
                                              new FixedPriceDetail()
                                              {
                                                  Type= PaymentMethodType.KM,
                                                  Price= 50,
                                                  CurrencyType= CurrencyType.TurkishLira,
                                                  StartDate= DateTime.Now,
                                                  EndDate= DateTime.Now.AddYears(1),
                                              }
                                          }
                                      }
                                    },

                                    IsActive= true,
                                    Order= 1,

                                    Quarters= new List<Quarter>()
                                    {
                                        new Quarter()
                                        {
                                            Name= "Test",
                                            IsActive= true,
                                            Order= 1
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }

            _context.SaveChanges();


            var hasCompany = _context.Companies.IgnoreQueryFilters().Include(x=>x.OrganizationUnitFk).Any(x => x.OrganizationUnitFk.Code== "00001");
            if (!hasCompany)
            {
                _context.Companies.Add(new()
                {
                    OrganizationUnitFk=new OrganizationUnit()
                    {
                        DisplayName="Test Şirket - 1",
                        Code= "00001",
                        TenantId= 1,
                    },
                    CityId= _context.Cities.FirstOrDefault().Id,
                    QuarterId =  _context.Quarters.FirstOrDefault().Id,
                    Address = "Test adres",
                    Email= "test@test.com",
                    Fax= "000000",
                    Phone= "999999999",
                    IsActive = true,
                    TaxAdministration= "Ankara Vergi Takip",
                    TaxNo= "88888",
                    Prefix= "TŞ1",
                    Type= CompanyType.Bookstore,
                    Vehicles= new List<Vehicle>()
                    {
                        new Vehicle()
                        {
                            Brand= "Test",
                            Capactiy= 10,
                            Description= "Açıklama",
                            Plate= "06 AA 022",
                            TrackNo="1433",
                            Year= 1990
                        }
                    },
                    RunningCode= "001",
                    Website= "https://test.com",
                    Contracts= new List<Contract>()
                    {
                        new Contract()
                        {
                            Formule="TEST",
                            StartDate= DateTime.Now,
                            EndDate= DateTime.Now.AddYears(1),
                            RegionId= _context.Regions.First().Id
                        }
                    }
                    
                });
            }


            var hasFuneralType = _context.FuneralTypes.IgnoreQueryFilters().Any(x => x.Description == "Test");
            if (!hasFuneralType)
            {
                _context.FuneralTypes.Add(new()
                {
                    Description="Test",
                    IsDefault= true,
                });
            }

            var hasAirline = _context.AirlineCompanies.IgnoreQueryFilters().Any(x => x.Code == "THY");
            if (!hasAirline)
            {
                _context.AirlineCompanies.Add(new()
                {
                    Code = "THY",
                    Name = "TÜRK HAVA YOLLARI",
                    FlightPrefix = "THY",
                    LadingPrefix = "THY"
                });
            }

            var hasAirport = _context.Airports.IgnoreQueryFilters().Any(x => x.Code == "SBH");
            if (!hasAirport)
            {
                _context.Airports.Add(new()
                {
                    Code="SBH",
                    CityId= _context.Cities.First().Id,
                    CountryId= _context.Cities.First().CountryId,
                    IsActive= true,
                    Name= "Sabiha Gökcen Havalimanı",
                    Order= 1,
                    
                });
            }
        }
    }
}
