namespace HLS.Topup.Common
{
    public class ErrorConst
    {
        public enum ActivityErrorCodes : int
        {
            AccountInvalid = 100,//Tài khoản k hợp lệ
            CheckServiceEnable = 101,//Service k hoạt động
            CheckActiveAccount = 102,//Tài khoản bị khóa
            CheckVerifyAccount = 103,//Tài khoản chưa xác thực
            CheckBalance = 104,//Số dư không đủ
            CheckTimeStaff = 105,//K nằm trong khung giờ quy định
            CheckCategory = 106,// Cate k hoặt động
            CheckActiveAccountAgent = 107,//Tài khoản đại lý của nhân viên bị khóa
            CheckUserDeposit = 109,//Tài khoản nhân viên thực hiện chuyển, nạp tiền
            CheckLimitAmount = 110,//Hạn mức không đủ
            CheckBalanceStaff = 111,//Số dư đại lý không đủ - hạn mức nhân viên đủ
            CheckLimitProduct = 112,//hạn mức bán hàng sản phẩm
            CheckPaymentMethod = 113,
        }
    }
}
