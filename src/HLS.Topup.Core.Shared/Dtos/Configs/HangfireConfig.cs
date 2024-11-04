namespace HLS.Topup.Dtos.Configs
{
    public class HangfireConfig
    {
        public int TimeAutoUnLockProvider { get; set; }

        public class DeleteBinaryObject
        {
            public bool IsRun { get; set; }
            public int TimeRun { get; set; }
        }

        public class MinStockAirtime
        {
            public bool IsRun { get; set; }
            public int TimeRun { get; set; }
            public string Providers { get; set; }
        }
    }
}
