using TDV.Burial;

using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace TDV.Burial.Dtos
{
    public class FuneralDto : EntityDto
    {
        public string TransferNo { get; set; }

        public string MemberNo { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public long? TcNo { get; set; }

        public string PassportNo { get; set; }

        public string LadingNo { get; set; }

        public FuneralStatus Status { get; set; }

        public DateTime OperationDate { get; set; }

        public int TypeId { get; set; }

        public int ContactId { get; set; }

        public long OwnerOrgUnitId { get; set; }

        public long GiverOrgUnitId { get; set; }

        public long? ContractorOrgUnitId { get; set; }

        public long? EmployeePersonId { get; set; }

        public int? FuneralPackageId { get; set; }

        public int? ContractId { get; set; }

        public int? VehicleId { get; set; }

        public FuneralFlightDto Flight { get; set; }

        public FuneralAddresDto Address { get; set; }

        public List<FuneralDocumentDto> Documents { get; set; }

    }
}