﻿using System.Threading.Tasks;

namespace HLS.Topup.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}