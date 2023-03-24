using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TransferNoFilter { get; set; }

        public string MemberNoFilter { get; set; }

        public string NameFilter { get; set; }

        public string SurnameFilter { get; set; }

        public long? MaxTcNoFilter { get; set; }
        public long? MinTcNoFilter { get; set; }

        public string PassportNoFilter { get; set; }

        public string LadingNoFilter { get; set; }

        public int? StatusFilter { get; set; }

        public DateTime? MaxOperationDateFilter { get; set; }
        public DateTime? MinOperationDateFilter { get; set; }

        public string FuneralTypeDescriptionFilter { get; set; }

        public string ContactDisplayPropertyFilter { get; set; }

        public string OwnerOrganizationUnitDisplayNameFilter { get; set; }

        public string GiverOrganizationUnitDisplayNameFilter { get; set; }

        public string ContractorOrganizationUnitDisplayNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string FuneralPackageCodeFilter { get; set; }

        public string ContractFormuleFilter { get; set; }

        public string VehiclePlateFilter { get; set; }

    }
}