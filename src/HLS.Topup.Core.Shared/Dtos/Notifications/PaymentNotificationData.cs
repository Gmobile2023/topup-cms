namespace HLS.Topup.Dtos.Notifications
{
    public class SendNotificationData
    {
        public string PartnerCode { get; set; }
        public string StaffAccount { get; set; }
        public string TransType { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string TransCode { get; set; }
        public decimal Amount { get; set; }
    }

    public class NotificationAppData
    {
        public string Type { get; set; }
        public object Properties { get; set; }
    }
}
