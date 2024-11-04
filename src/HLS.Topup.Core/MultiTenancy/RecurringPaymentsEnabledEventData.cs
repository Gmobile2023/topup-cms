using Abp.Events.Bus;

namespace HLS.Topup.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}