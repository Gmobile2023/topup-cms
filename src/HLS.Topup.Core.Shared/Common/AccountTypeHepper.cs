namespace HLS.Topup.Common
{
    public static class AccountTypeHepper
    {
        public static bool IsAccountBackend(string accountType)
        {
            return accountType == CommonConst.SystemAccountType.System.ToString("G") ||
                   accountType == CommonConst.SystemAccountType.Sale.ToString("G") ||
                   accountType == CommonConst.SystemAccountType.SaleLead.ToString("G");
        }
        public static bool IsAccountBackend(CommonConst.SystemAccountType accountType)
        {
            return accountType == CommonConst.SystemAccountType.System ||
                   accountType == CommonConst.SystemAccountType.Sale ||
                   accountType == CommonConst.SystemAccountType.SaleLead;
        }
    }
}