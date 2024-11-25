namespace HLS.Topup.Common
{
    public class ResponseMessageBase
    {
        public string Code { get; set; }
        public ResponseMessageBase()
        {
            Code = "0";
        }
        public string Message { get; set; }
        public string ExtraInfo { get; set; }
        public string PayLoad { get; set; }
    }
}
