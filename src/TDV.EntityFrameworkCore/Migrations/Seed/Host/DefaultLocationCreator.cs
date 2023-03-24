using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TDV.EntityFrameworkCore;
using TDV.Location;
using TDV.Payment;

namespace TDV.Migrations.Seed.Host
{
    public class DefaultLocationCreator
    {
        private readonly TDVDbContext _context;

        public DefaultLocationCreator(TDVDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var getCountry = _context.Countries.FirstOrDefault(x => x.Name == "Türkiye");
            if (getCountry != null)
            {
                return;
            }

            var getRegion = _context.Regions.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Türkiye");
            if (getRegion==null)
            {
                _context.Regions.Add(new()
                {
                    Name="Türkiye",
                    FixedPriceFk= new()
                    {
                        Name= "Türkiye sabit fiyatlandırması",
                        Details = new List<FixedPriceDetail>(){
                            new()
                            {
                                Type= PaymentMethodType.FuneralCount,
                                Price= 250,
                                StartDate= DateTime.Now,
                                EndDate= DateTime.Now.AddDays(365)
                            }
                        }
                    }
                });

                _context.SaveChanges();

                getRegion= _context.Regions.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Türkiye");
            }

            _context.Countries.Add(new()
            {
                Name = "Türkiye",
                Code = "90"
            });
            _context.SaveChanges();

            getCountry = _context.Countries.FirstOrDefault(x => x.Name == "Türkiye");

            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations/Seed/Files/TurkeyLocations.json");
            var data= JArray.Parse(File.ReadAllText(file));

            foreach (var il in data)
            {
                var city = new City()
                {
                    Name = il["ADI"].Value<string>(),
                    CountryId = getCountry.Id,
                    Code = il["ID"].Value<string>(),
                    Districts = il["ILCES"].Value<JArray>()
                    .Select(x =>
                        new District
                        {
                            Name = x["ADI"].Value<string>(),
                            RegionId = getRegion.Id,
                            Quarters = x["MAHALLES"].Value<JArray>().Select(y =>
                            new Quarter
                            {
                                Name = y["ADI"].Value<string>(),
                                
                            }).ToList()
                        }
                    ).ToList()
                };

                _context.Cities.Add(city);
                _context.SaveChanges();
            }
        }
    }
}
