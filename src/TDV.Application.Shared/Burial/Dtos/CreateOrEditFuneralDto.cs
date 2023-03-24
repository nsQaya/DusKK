using TDV.Burial;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TDV.Communication.Dtos;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralDto : EntityDto<int?>
    {

        [Required]
        [StringLength(FuneralConsts.MaxTransferNoLength, MinimumLength = FuneralConsts.MinTransferNoLength)]
        public string TransferNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxMemberNoLength, MinimumLength = FuneralConsts.MinMemberNoLength)]
        public string MemberNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxNameLength, MinimumLength = FuneralConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxSurnameLength, MinimumLength = FuneralConsts.MinSurnameLength)]
        public string Surname { get; set; }

        public long? TcNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxPassportNoLength, MinimumLength = FuneralConsts.MinPassportNoLength)]
        public string PassportNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxLadingNoLength, MinimumLength = FuneralConsts.MinLadingNoLength)]
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

        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? QuarterId { get; set; }

        public CreateOrEditContactDto Contact { get; set; }
    }
}