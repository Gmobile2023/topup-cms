﻿using System.Runtime.Serialization;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    public class VtpRequest
    {
        [DataContract]
        [Route("/api/get-otp")]
        public class GetOtpRequest
        {
            [DataMember(Name = "msisdn")] public string Msisdn { get; set; }
        }

        [DataContract]
        [Route("/api/get-captcha")]
        public class GetCaptchaRequest
        {
        }

        [DataContract]
        [Route("/api/register-user-by-phone")]
        public class RegisterMyViettelRequest
        {
            [DataMember(Name = "isdn")] public string Isdn { get; set; }

            [DataMember(Name = "password")] public string Password { get; set; }

            [DataMember(Name = "password_confirmation")]
            public string PasswordConfirmation { get; set; }

            [DataMember(Name = "otp")] public string Otp { get; set; }

            [DataMember(Name = "device_id")] public string DeviceId { get; set; }

            [DataMember(Name = "regType")] public object RegType { get; set; }

            [DataMember(Name = "listAcc")] public string ListAcc { get; set; }

            [DataMember(Name = "captcha")] public string Captcha { get; set; }

            [DataMember(Name = "isWeb")] public byte IsWeb { get; set; }

            [DataMember(Name = "captcha_code")] public string CaptchaCode { get; set; }

            [DataMember(Name = "sid")] public string Sid { get; set; }
        }

        [DataContract]
        [Route("/api/tranferMoney")]
        public class TransferMoneyRequest
        {
            [DataMember(Name = "sid")] public string Sid { get; set; }

            [DataMember(Name = "captcha")] public string Captcha { get; set; }

            [DataMember(Name = "receiver")] public string Receiver { get; set; }

            [DataMember(Name = "amount")] public long Amount { get; set; }

            [DataMember(Name = "passwd")] public string Passwd { get; set; }
        }
    }
}