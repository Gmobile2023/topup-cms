using System.Collections.Generic;
using HLS.Topup.Transactions.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.TransactionManagement
{
    public class TransactionManagementViewModel
    {
        public List<TransactionsProviderLookupTableDto> ProviderList { get; set; }
        public List<TransactionsServiceLookupTableDto> TransactionsServiceList { get; set; }
    }

    public class TransactionDetailViewDto
    {
        public string TransCode { get; set; }
    }
}