using TDV.Payment.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.CompanyTransactions
{
    public class CreateOrEditCompanyTransactionModalViewModel
    {
        public CreateOrEditCompanyTransactionDto CompanyTransaction { get; set; }

        public string CompanyTaxAdministration { get; set; }

        public string FuneralDisplayProperty { get; set; }

        public string DataListValue { get; set; }

        public string CurrencyCode { get; set; }

        public string DataListValue2 { get; set; }

        public List<CompanyTransactionCompanyLookupTableDto> CompanyTransactionCompanyList { get; set; }

        public List<CompanyTransactionFuneralLookupTableDto> CompanyTransactionFuneralList { get; set; }

        public List<CompanyTransactionDataListLookupTableDto> CompanyTransactionDataListList { get; set; }

        public List<CompanyTransactionCurrencyLookupTableDto> CompanyTransactionCurrencyList { get; set; }

        public bool IsEditMode => CompanyTransaction.Id.HasValue;
    }
}