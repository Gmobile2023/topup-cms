using System.Collections.Generic;
using HLS.Topup.Web.Models.TopupRequest;

namespace HLS.Topup.Web.Models
{
    public class DepositInfoModel
    {
        public decimal Amount { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public string BranchName { get; set; }

        public string Description { get; set; }
        public string Account { get; set; }
    }
    public class PayInfoModel
    {
        public string CardTelco { get; set; }
        public string PhoneNumber { get; set; }
        public decimal CardPrice { get; set; }
        public string ProductCode { get; set; }

        public string Email { get; set; }
        public int Quantity { get; set; }        
        public List<TopupListItemDto> ListNumbers { get; set; }        
    }
    public class TopupInfoDto
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Method { get; set; }
        public string PhoneNumber { get; set; }
        public decimal CardValue { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal FixAmount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }

        public string Email { get; set; }
        public int Quantity { get; set; }
        public List<TopupListItemDto> ListNumbers { get; set; }

        public string Description { get; set; }

        public string ServiceCode { get; set; }
    }



}
