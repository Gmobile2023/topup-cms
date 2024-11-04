namespace HLS.Topup.Common.Dto
{
    public class SmsReceiverDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public string Gateway { get; set; }
    }
}