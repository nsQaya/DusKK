namespace TDV.Corporation.Dtos
{
    public class GetCompanyForViewDto
    {
        public CompanyDto Company { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string CityDisplayProperty { get; set; }

        public string QuarterName { get; set; }

    }
}