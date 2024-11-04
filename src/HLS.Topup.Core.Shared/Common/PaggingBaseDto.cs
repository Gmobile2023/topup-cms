namespace HLS.Topup.Common
{
    public class PaggingBaseDto
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Order { get; set; }
        public SearchType SearchType { get; set; }
        public int RowMax { get; set; }
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }
    }

    public enum SearchType : byte
    {
        Search = 1,
        Export = 2
    }
}