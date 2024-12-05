namespace HLS.Topup.Common
{
    public class ResponseCodeConst
    {
        public const string Success = "1"; //Thành công
        public const string Error = "0"; //Lỗi
        public const string ResponseCode_Success = "1";
        public const string ResponseCode_00 = "0";
        public const string ResponseCode_RequestReceived = "4000";//Đã tiếp nhận giao dịch
        public const string ResponseCode_RequestAlreadyExists = "4001";//Giao dịch đối tác đã tồn tại
        public const string ResponseCode_Cancel = "4002";
        public const string ResponseCode_Failed = "4003";
        public const string ResponseCode_TransactionNotFound = "4004";//Giao dịch không tồn tại
        public const string ResponseCode_TimeOut = "4005";
        public const string ResponseCode_InProcessing = "4006";
        public const string ResponseCode_WaitForResult = "4007";
        public const string ResponseCode_Paid = "4008";
        public const string ResponseCode_CardNotInventory = "4009";
        public const string ResponseCode_Balance_Not_Enough = "6001";
    }
}
