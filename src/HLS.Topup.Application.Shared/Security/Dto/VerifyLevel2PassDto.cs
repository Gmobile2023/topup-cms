namespace HLS.Topup.Security.Dto
{
    public class VerifyLevel2PassDto
    {
        public string Password { get; set; }
    }
    public class UpdateLevel2PassDto
    {
        public string Password { get; set; }
    }
    public class ChangeLevel2PassDto
    {
        public string Password { get; set; }
        public string Otp { get; set; }
    }

    public class UpdateMobileSendOtp
    {
        public string PhoneNumber { get; set; }
    }
}
