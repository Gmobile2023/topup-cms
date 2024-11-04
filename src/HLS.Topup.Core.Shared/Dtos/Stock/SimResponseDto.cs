using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class SimResponseDto
    {
        public string Id { get; set; }
        public string SimNumber { get; set; }
        public string Iccid { get; set; }
        public DateTime CreatedDate { get; set; }
        public CommonConst.SimStatus Status { get; set; }
        public int TransTimesInDay { get; set; }
        public DateTime LastTransTime { get; set; }
        public int SimBalance { get; set; }
        public string StockType { get; set; }
        public int ComPort { get; set; }
        public int BaudRate { get; set; }
        public bool IsInprogress { get; set; }
        public string WorkerAppName { get; set; }
        public virtual bool IsSimPostpaid { get; set; }

        public string MyViettelPass { get; set; }

        public CommonConst.SimAppType? SimAppType { get; set; }
    }
}