using TDV.Burial;
using TDV.Communication;
using Abp.Organizations;
using Abp.Organizations;
using Abp.Organizations;
using TDV.Authorization.Users;
using TDV.Burial;
using TDV.Payment;
using TDV.Corporation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace TDV.Burial
{
    [Table("Funerals")]
    [Audited]
    public class Funeral : FullAuditedEntity
    {

        [Required]
        [StringLength(FuneralConsts.MaxTransferNoLength, MinimumLength = FuneralConsts.MinTransferNoLength)]
        public virtual string TransferNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxMemberNoLength, MinimumLength = FuneralConsts.MinMemberNoLength)]
        public virtual string MemberNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxNameLength, MinimumLength = FuneralConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxSurnameLength, MinimumLength = FuneralConsts.MinSurnameLength)]
        public virtual string Surname { get; set; }

        public virtual long? TcNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxPassportNoLength, MinimumLength = FuneralConsts.MinPassportNoLength)]
        public virtual string PassportNo { get; set; }

        [Required]
        [StringLength(FuneralConsts.MaxLadingNoLength, MinimumLength = FuneralConsts.MinLadingNoLength)]
        public virtual string LadingNo { get; set; }

        public virtual FuneralStatus Status { get; set; }

        public virtual DateTime OperationDate { get; set; }

        public virtual int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public FuneralType TypeFk { get; set; }

        public virtual int ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long OwnerOrgUnitId { get; set; }

        [ForeignKey("OwnerOrgUnitId")]
        public OrganizationUnit OwnerOrgUnitFk { get; set; }

        public virtual long GiverOrgUnitId { get; set; }

        [ForeignKey("GiverOrgUnitId")]
        public OrganizationUnit GiverOrgUnitFk { get; set; }

        public virtual long? ContractorOrgUnitId { get; set; }

        [ForeignKey("ContractorOrgUnitId")]
        public OrganizationUnit ContractorOrgUnitFk { get; set; }

        public virtual long? EmployeePersonId { get; set; }

        [ForeignKey("EmployeePersonId")]
        public User EmployeePersonFk { get; set; }

        public virtual int? FuneralPackageId { get; set; }

        [ForeignKey("FuneralPackageId")]
        public FuneralPackage FuneralPackageFk { get; set; }

        public virtual int? ContractId { get; set; }

        [ForeignKey("ContractId")]
        public Contract ContractFk { get; set; }

        public virtual int? VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle VehicleFk { get; set; }
        public List<FuneralFlight> Flights { get; set; }

        public List<FuneralDocument> Documents { get; set; }

        public List<FuneralAddres> Addresses { get; set; }
    }
}