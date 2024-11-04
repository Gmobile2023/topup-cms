using System;

namespace HLS.Topup.Utils
{
    public static class DateTimeHelper
    {
        public static long ConvertToTimestamp(DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = dateTime - epoch;
            return (long)timeSpan.TotalSeconds;
        }

        public static DateTime ConvertToDateTime(long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(timestamp);
        }
    }
}