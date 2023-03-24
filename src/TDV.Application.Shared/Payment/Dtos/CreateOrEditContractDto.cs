using TDV.Payment;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class CreateOrEditContractDto : EntityDto<int?>
    {

        public string Formule { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public int RegionId { get; set; }

        public int CompanyId { get; set; }

    }
}