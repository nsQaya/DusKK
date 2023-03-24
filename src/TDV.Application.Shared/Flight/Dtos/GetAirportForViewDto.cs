namespace TDV.Flight.Dtos
{
    public class GetAirportForViewDto
    {
        public AirportDto Airport { get; set; }

        public string CountryDisplayProperty { get; set; }

        public string CityDisplayProperty { get; set; }

    }
}