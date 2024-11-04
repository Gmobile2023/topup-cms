using HLS.Topup.Common;
using JetBrains.Annotations;

namespace HLS.Topup.RequestDtos
{
    public interface IUserInfoRequest
    {
        string PartnerCode { get; set; }
        string ParentCode { get; set; }
        string StaffAccount { get; set; }
        string StaffUser { get; set; }
        CommonConst.SystemAccountType AccountType { get; set; }
        CommonConst.AgentType AgentType { get; set; }
    }

}
