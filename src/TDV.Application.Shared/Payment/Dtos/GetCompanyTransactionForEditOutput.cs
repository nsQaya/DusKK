using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class GetCompanyTransactionForEditOutput
    {
        public CreateOrEditCompanyTransactionDto CompanyTransaction { get; set; }

        public string CompanyTaxAdministration { get; set; }

        public string FuneralDisplayProperty { get; set; }

        public string DataListValue { get; set; }

        public string CurrencyCode { get; set; }

        public string DataListValue2 { get; set; }

    }
}