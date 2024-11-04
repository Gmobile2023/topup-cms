using System;

namespace HLS.Topup.Web.Authentication.TwoFactor
{
    [Serializable]
    public class TwoFactorCodeCacheItem
    {
        public const string CacheName = "AppTwoFactorCodeCache";
        public static readonly TimeSpan DefaultSlidingExpireTime = TimeSpan.FromMinutes(2);
        public string Code { get; set; }

        public TwoFactorCodeCacheItem()
        {
            
        }

        public TwoFactorCodeCacheItem(string code)
        {
            Code = code;
        }
    }
}