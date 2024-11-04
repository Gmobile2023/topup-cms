using HLS.Topup.Common;

namespace HLS.Topup.Web.Views.Shared.Components.AccountInfo
{
    public class AccountInfoHeaderModel
    {
        public CommonConst.SystemAccountType AccountType { get; set; }
        public string AccountCode { get; set; }
        public string StaffAccount { get; set; }
        public string Balance { get; set; }
        //public bool IsUserStaff { get; set; }

    }
}
