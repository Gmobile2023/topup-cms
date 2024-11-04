using System.Collections.Generic;

namespace HLS.Topup.Dtos.Configuration
{
    public class ServiceConfiguationDto
    {
        public virtual string ProviderCode { get; set; }
        public virtual string ProviderName { get; set; }
        public virtual string ExtraInfo { get; set; }
        public virtual string Name { get; set; }

        /// <summary>
        /// Địa chỉ url api
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// Passwork kết nối API
        /// </summary>
        public virtual string ApiPass { get; set; }

        /// <summary>
        /// Tài khoản kết nối API
        /// </summary>
        public virtual string ApiAccount { get; set; }

        /// <summary>
        /// API Key
        /// </summary>
        public virtual string ApiKey { get; set; }

        /// <summary>
        /// Cấu hình timeout gọi đối tác
        /// </summary>
        public virtual int? TimeOut { get; set; }

        /// <summary>
        /// Số lần retry
        /// </summary>
        public virtual byte? Retry { get; set; }

        /// <summary>
        /// Thời gian sleep giữa các lần retry
        /// </summary>
        public virtual int? SleepRetry { get; set; }

        /// <summary>
        /// Thời gian gọi lại check giao dịch
        /// </summary>

        public virtual int? TimeAwaitCheckTrans { get; set; }

        public virtual int? MaxConnection { get; set; }
        public virtual string Description { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public bool IsOpened { get; set; }
        public int Priority { get; set; }
        public string AccountCode { get; set; }
        public string ProductCode { get; set; }
        public string TransCodeConfig { get; set; }
        public decimal? ProductValue { get; set; }
        public bool IsSlowTrans { get; set; }

        public int? ProviderSetTransactionTimeout { get; set; }
        public int? ProviderMaxWaitingTimeout { get; set; }
        public bool? IsEnableResponseWhenJustReceived { get; set; }
        public string StatusResponseWhenJustReceived { get; set; }

        public int? WaitingTimeResponseWhenJustReceived { get; set; }
        public bool IsLastConfiguration { get; set; }
        public string ParentProvider { get; set; }
        public bool IsAutoDeposit { get; set; }
        public bool IsRoundRobinAccount { get; set; }
        public decimal MinBalance { get; set; }
        public decimal MinBalanceToDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public List<ServiceConfiguationDto> SubConfiguration { get; set; }
        public string AllowTopupReceiverType { get; set; }
        public decimal RateRunning { get; set; }
        public string WorkShortCode { get; set; }
    }
}