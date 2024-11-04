using System;

namespace HLS.Topup.Common
{
    public static class StringExtensions
    {
        public static string ToFormat(this string s, string p)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                {
                    s = "0";
                }
                var n = Convert.ToDouble(s);
                return Math.Round(n).ToString("N0").Replace(@",", ".") + p;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string ToFormatCustom(this string s, string p)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                {
                    s = "0";
                }
                var n = decimal.Parse(s.ToString().Replace(".", ",") ?? string.Empty);
                return Math.Round(n).ToString("N0").Replace(@",", ".") + p; //1,000,000.235
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string ToFormat(this string s)
        {
            return s.ToFormat("đ");
        }

        public static string ToFormat(this decimal s)
        {
            return s.ToString("###").ToFormat();
        }

        public static string ToFormat(this decimal s, string p)
        {
            return s.ToString("###").ToFormat(p);
        }
      

        public static string ToFormat(this decimal? s)
        {
            return s.Value.ToFormat();
        }


        public static string ToFormat(this decimal? s, string p)
        {
            return s.Value.ToFormat(p);
        }

        public static string ToFormat(this int? s, string p)
        {
            return s.Value.ToString("###").ToFormat(p);
        }

        public static string ToFormat(this int s, string p)
        {
            return s.ToString("###").ToFormat(p);
        }

        public static string ToFormat(this int s)
        {
            return s.ToString("###").ToFormat();
        }
        
        public static string ToFormat(this float s, string p)
        {
            return s.ToString("###").ToFormat(p);
        }
        public static string ToFormat(this float s)
        {
            return s.ToString("###").ToFormat();
        }

        public static string Nl2Br(this string htmlHelper)
        {
            return htmlHelper.Replace(@"\n", "<br />")
                .Replace(@"\r", "<br />");
        }
    }


}
