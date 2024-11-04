using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Users.Dto
{
    public class GetUserInfoRequest
    {
        public string AccountCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long? UserId { get; set; }
        public string Search { get; set; }
        public string UserName { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    public class GetInfoSaleRequest
    {
        public string AccountCode { get; set; }     
    }



    public class GetNetworkAccount
    {
        public string AccountCode { get; set; }
        public int Level { get; set; }
    }

    public class GetLimitRequest
    {
        public string AccountCode { get; set; }
    }

    public class GetUserPeriodRequest
    {
        public string AgentCode { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    public class GetTestRequest
    {       
    }

}
