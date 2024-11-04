namespace HLS.Topup.Common
{
    public class CommonConst
    {
        public enum Channel : byte
        {
            WEB = 1,
            APP = 2,
            API = 3
        }

        public enum DepositStatus : byte
        {
            Pending = 0,
            Approved = 1,
            Canceled = 3,
            Deleted = 4,
            Error = 5,
            Processing = 6
        }

        public enum DepositType : byte
        {
            Deposit = 1,
            Increase = 2,
            Decrease = 3,
            SaleDeposit = 4,
            Cash = 5
        }

        // public enum SaleManType : byte
        // {
        //     SaleStaff = 1,
        //     SaleLead = 2
        // }
        public enum SaleManStatus : byte
        {
            Active = 1,
            Lock = 2
        }

        public enum DebtLimitAmountStatus
        {
            Init = 0,
            Active = 1,
            Lock = 2
        }

        public enum ClearDebtType
        {
            CashOnHand = 1,
            CashInBank = 2
        }

        public enum ClearDebtStatus
        {
            Init = 0,
            Approval = 1,
            Cancel = 2
        }

        public enum AdjustmentType : byte
        {
            Increase = 1,
            Decrease = 2
        }

        public class IncrementCodeKey
        {
            public static readonly string DiscountCode = "Paygate_DiscountCodeWeb_Key";
            public static readonly string DepositCode = "Paygate_DepositCodeWeb_Key";
            public static readonly string TopupCode = "Paygate_TopupCodeWeb_Key";
        }

        public class SmsChannel
        {
            public static readonly string MobileNetBrandName = "MobileNetBrandName";
            public static readonly string MogileGo = "MogileGo";
            public static readonly string MobileNet = "MobileNet";
        }

        public class TelcoConst
        {
            public static readonly string Viettel = "VTE";
            public static readonly string Vinaphone = "VNA";
            public static readonly string Mobiphone = "VMS";
            public static readonly string Gmobile = "GMOBILE";
            public static readonly string VietNammobile = "VNM";
        }

        public class CategoryCodeConts
        {
            public static readonly string MOBILE_BILL = "MOBILE_BILL";
            public static readonly string EVN_BILL = "EVN_BILL";
        }

        public class PaymentInfoType
        {
            public static readonly string Success = "Success";
            public static readonly string Error = "Error";
        }

        public enum TransactionType : byte
        {
            Default = 0,
            Transfer = 1,
            Deposit = 2,
            Cashout = 3,
            Payment = 4,
            Revert = 5,
            MasterTopup = 6,
            MasterTopdown = 7,
            CorrectUp = 8,
            CorrectDown = 9,
            Block = 10,
            Unblock = 11,
            Topup = 12,
            Tkc = 13,
            PinCode = 14,
            CollectDiscount = 15,
            FeePriority = 16,
            CancelPayment = 17,
            CardCharges = 18,
            AdjustmentIncrease = 19,
            AdjustmentDecrease = 20,
            ClearDebt = 21,
            SaleDeposit = 22,
            Received = 25,
            PayBatch = 26
        }

        public enum TransStatus : byte
        {
            Init = 0,
            Done = 1,
            Cancel = 2,
            Error = 3,
            Reverted = 4,
            PartialRevert = 5,
            CorrectUp = 6,
            CorrectDown = 7
        }

        public enum CardPackageStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Lock = 2, //Khóa
            // Delete = 3, //Xóa
            // Undefined = 99, //Chưa xác định
        }

        public enum CardStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Exported = 2, //Đã xuất kho
            Delete = 3, //Xóa
            Cancelled = 4, //Hủy
            OnExchangeMode = 11, //Đang chuyển kho
            Undefined = 99, //Chưa xác định
        }

        public enum CardRequestStatus : byte
        {
            Init = 0,
            Success = 1,
            Canceled = 2,
            Failed = 3,
            TimeOver = 4,
            InProcessing = 6,
            ProcessTimeout = 7,
            InvalidCardValue = 8,
            Undefined = 99
        }

        public enum CardStockStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Lock = 2, //Khóa
            Delete = 3, //Xóa
            Undefined = 99, //Chưa xác định
        }

        public enum SimStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Lock = 2, //Khóa
            Delete = 3, //Xóa
            Undefined = 99 //Chưa xác định
        }

        public enum TopupStatus
        {
            Init = 0,
            Success = 1,
            Canceled = 2,
            Failed = 3,
            TimeOver = 4,
            InProcessing = 6,
            ProcessTimeout = 7,
            WaitForResult = 8,
            Paid = 9, //Đã thanh toán
            WaitForConfirm = 10, //Trạng thái gd chậm. chờ nạp bù và kết luận bằng tay
            Undefined = 99
        }

        public enum CardBatchType : byte
        {
            CardSale = 1,
            CardMapping = 2,
            MappingCanSale = 3
        }

        public enum SimAppType : byte
        {
            Modem = 1,
            MyViettel = 2,
            ViettelPay = 3,
            ViettelPayPro = 4
        }

        public enum CategoryStatus : byte
        {
            Init = 0,
            Active = 1,
            Lock = 2
        }

        public enum VerifyTransType : byte
        {
            LevelPassword = 1,
            Odp = 2,
            Otp = 3,
            Biometrics = 4,
            Fingerprint = 5,
            FaceId = 6,
            None = 0
        }

        public enum CategoryType : byte
        {
            Airtime = 1,
            PinCode = 2,
            Data = 3
        }

        public enum ProductStatus : byte
        {
            Init = 0,
            Active = 1,
            Lock = 2
        }

        public enum ProductType : byte
        {
            Airtime = 1,
            PinCode = 2,
            Data = 3
        }

        public enum ProviderStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Lock = 2 //Khóa
        }

        public enum ProviderType : byte
        {
            Telco = 1,
            PinCode = 2,
            Data = 3
        }

        public static class ServiceCodes
        {
            public static string TOPUP = "TOPUP";
            public static string TKC = "TKC";
            public static string PAYMENT_SMS_GAME = "PAYMENT_SMS_GAME";
            public static string PIN_CODE = "PIN_CODE";
            public static string PIN_DATA = "PIN_DATA";
            public static string PIN_GAME = "PIN_GAME";
            public static string TOPUP_DATA = "TOPUP_DATA";
            public static string PAY_BILL = "PAY_BILL";
            public static string QUERY_BILL = "QUERY_BILL";
            public static string TOPUP_BATCH = "TOPUP_BATCH";
            public static string TRANSFER_MONEY = "TRANSFER_MONEY";
            public static string DEPOSIT = "DEPOSIT";
            public static string REFUND = "REFUND";
            public static string PAYBATCH = "PAYBATCH";
            public static string PAYCOMMISSION = "PAYCOMMISSION";
            public static string FEE_PRIORITY = "FEE_PRIORITY";
        }

        public static class MessageType
        {
            public static string ServiceDisible = "ServiceDisible";
            public static string StaffOutTime = "StaffOutTime";
        }

        public enum DiscountStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Applying = 2, //Đang áp dụng
            NotApply = 4, //Chưa ap dụng
            StopApply = 5, //Ngừng áp dụng
            Delete = 6 //Ngừng áp dụng
        }

        public enum DiscountType : byte
        {
            DiscountSystem = 1,
            DiscountNetwork = 2,
        }

        public enum FeeStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Applying = 2, //Đang áp dụng
            NotApply = 4, //Chưa ap dụng
            StopApply = 5, //Ngừng áp dụng
            Delete = 6 //Ngừng áp dụng
        }

        public enum DiscountOrderCdnStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Applying = 2, //Đang áp dụng
            NotApply = 4, //Chưa ap dụng
            StopApply = 5 //Ngừng áp dụng
        }

        public enum LevelDiscountStatus : byte
        {
            Default = 99,
            Init = 0,
            Payment = 1,
            Cancel = 3,
            Fail = 4
        }

        public enum SystemAccountType : byte
        {
            System = 0, //hệ thống
            Company = 1, //công ty
            MasterAgent = 2, //Đại lý
            Agent = 3, //Đại lý tuyến dưới
            Staff = 4, //nhân viên
            SaleLead = 5, //sale lead
            Sale = 6, //sale
            StaffApi = 7, //nv api
            Default = 99
        }

        public enum AgentType : byte
        {
            Agent = 1, //Đại lý
            AgentApi = 2, //Đại lý bán hàng qua api
            AgentCampany = 3, //Đại lý có hệ thống riêng
            AgentGeneral = 4, //Đại lý tổng
            SubAgent = 5, //Đại lý con
            WholesaleAgent = 6, //Đại sỉ
            Default = 99
        }

        public enum SaleType : byte
        {
            Normal = 0,
            Slow = 1,
            Default = 99
        }

        public enum BankStatus : byte
        {
            Active = 1, //Đại lý
            Lock = 2
        }

        public enum OtpStatus
        {
            Init = 0,
            Success = 1,

            //Confirm = 2,
            Timeout = 3,
            Cancel = 4
        }

        public enum OtpType
        {
            Transfer = 1,
            PayBill = 2,
            Payment = 3,
            ResetPass = 4,
            ChangePassLevel2 = 5,
            Register = 6,
            Login = 7,
            ChangePaymentMethod = 8
        }

        public enum CountryStatus
        {
            Active = 1,
            Lock = 0
        }

        public enum CityStatus
        {
            Active = 1,
            Lock = 0
        }

        public enum DistrictStatus
        {
            Active = 1,
            Lock = 0
        }

        public enum WardStatus
        {
            Active = 1,
            Lock = 0
        }

        public enum IdType
        {
            IdentityCard = 1, //CMND
            CitizenIdentity = 2, //Căn cước
            Passport = 3 //Hộ chiếu
        }

        public enum BotMessageType
        {
            Error = 1,
            Wraning = 2,
            Message = 3
        }

        public enum BotType
        {
            Dev = 1,
            Sale = 2,
            CardMapping = 3,
            Provider = 4,
            Transaction = 5,
            Stock = 6,
            Deposit = 7,
            Channel = 8
        }

        public enum PayBackStatus : byte
        {
            Init = 0,
            Approval = 2,
            Cancel = 3,
            Error = 4,
            Processing = 5
        }

        public enum LimitProductConfigStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Applying = 2, //Đang áp dụng
            NotApply = 4, //Chưa ap dụng
            StopApply = 5, //Ngừng áp dụng
            Delete = 6 //Ngừng áp dụng
        }

        public enum BlockBalanceStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
        }

        public enum BlockBalanceType : byte
        {
            Block = 1,
            UnBlock = 2
        }

        public enum PayBatchBillStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Error = 4,
            Processing = 5
        }

        public enum SystemTransferStatus : byte
        {
            Pending = 0, //chờ duyệt
            Approved = 1, //đã duyệt
            Cancel = 3, //Đã hủy
            Error = 4, //Đã hủy
            Processing = 5, //Đã hủy
        }

        public enum AccountActivityType
        {
            Default = 99,
            AssignSale = 1,
            ConvertSale = 2,
            UpdateSale = 3,
            Lock = 4,
            UnLock = 5,
            ChangeUserName = 6,
        }

        public enum PayBillCustomerStatus : byte
        {
            Default = 99,
            Unpaid = 0,
            Paid = 1
        }

        public enum SendNotificationStatus : byte
        {
            Pending = 0,
            Approved = 1,
            Cancel = 3,
            Published = 2,
            Failed = 4
        }

        public enum SaleRequestType : byte
        {
            Topup = 1,
            TopupPartner = 2,
            PayBill = 3,
            PinCode = 4,
            TopupList = 5,
            PayBillList = 6
        }


        public enum BatchLotRequestStatus : byte
        {
            Init = 0,
            Completed = 1,
            Process = 2,
            Stop = 3
        }

        public enum SaleRequestStatus : byte
        {
            Init = 0,
            Success = 1,
            Canceled = 2,
            Failed = 3,
            TimeOver = 4,
            InProcessing = 6,
            ProcessTimeout = 7,
            WaitForResult = 8,
            Paid = 9, //Đã thanh toán
            Mapping = 10, //GHép dc 1 phần
            Undefined = 99
        }

        public enum AgentPartnerContactInfoType
        {
            Director = 1,
            Technical = 2,
            Comparator = 3,
            Accountant = 4,
        }

        public enum MethodReceivePassFile : byte
        {
            Sms = 1,
            Email = 2,
        }

        public enum PartnerServiceConfigurationStatus : byte
        {
            Init = 0, //Khởi tạo
            Active = 1, //Hoạt động
            Lock = 2 //Khóa
        }
        
        public enum LowBalanceAlert : byte
        {
            Init = 0,
            Active = 1,
        }
    }
}
