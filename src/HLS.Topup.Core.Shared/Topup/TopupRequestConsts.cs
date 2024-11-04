namespace HLS.Topup.Topup
{
    public class TopupRequestConsts
    {

        public const int MinMobileNumberLength = 0;
        public const int MaxMobileNumberLength = 20;

        public const int MinPartnerCodeLength = 0;
        public const int MaxPartnerCodeLength = 256;

        public const int MinTransRefLength = 0;
        public const int MaxTransRefLength = 256;

        public const int MinTransCodeLength = 0;
        public const int MaxTransCodeLength = 256;

    }
    public enum MultiplesTopupConts: int
    {
        All = 0,
        Value50LessThan = 10000,
        Value20 = 20000,
        Value50 = 50000,
        Value100 = 100000,
        Value200 = 200000,
        Value300 = 300000,
        Value500 = 500000,
        Value1000000 = 1000000
    }
}