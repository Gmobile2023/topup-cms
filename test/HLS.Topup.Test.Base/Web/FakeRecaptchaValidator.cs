using System.Threading.Tasks;
using HLS.Topup.Security.Recaptcha;

namespace HLS.Topup.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
