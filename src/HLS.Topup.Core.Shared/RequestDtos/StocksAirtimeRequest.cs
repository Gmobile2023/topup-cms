using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HLS.Topup.Common;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{

    [Route("/api/v1/stock/airtime-list", "GET")]
    public class GetAllStockAirtimeRequest : PaggingBaseDto,IUserInfoRequest
    {
        public string Filter { get; set; }
        public string ProviderCode { get; set; }
        public byte Status { get; set; }


        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime", "POST")]
    public class CreateStockAirtimeRequest : IUserInfoRequest
    {
        public string ProviderCode { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public decimal MaxLimitAirtime { get; set; }
        public decimal MinLimitAirtime { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime", "PUT")]
    public class UpdateStockAirtimeRequest : IUserInfoRequest
    {
        public string ProviderCode { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public decimal MaxLimitAirtime { get; set; }
        public decimal MinLimitAirtime { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/topupgate/balance", "GET")]
    public class GetAvailableStockAirtimeRequest : IUserInfoRequest
    {
        public string ProviderCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/topupgate/topup/viettel/deposit", "POST")]
    public class ViettelDepositRequest
    {
        public decimal Amount { get; set; }

        public string ProviderCode { get; set; }
    }

    [Route("/api/v1/stock/airtime", "GET")]
    public class GetStockAirtimeRequest : IUserInfoRequest
    {
        public string ProviderCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }



    [Route("/api/v1/stock/airtime-import-list", "GET")]
    public class GetAllBatchAirtimeRequest : PaggingBaseDto,IUserInfoRequest
    {
        public string Filter { get; set; }
        public string BatchCode { get; set; }
        public string ProviderCode { get; set; }
        public byte Status { get; set; }
        public DateTime? FormDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime-import", "POST")]
    public class CreateBatchAirtimeRequest : IUserInfoRequest
    {
        private IUserInfoRequest _userInfoRequestImplementation;
        public string ProviderCode { get; set; }
        public decimal Amount { get; set; }
        public float Discount { get; set; }
        public decimal Airtime { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public string CreatedAccount { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime-import", "PUT")]
    public class UpdateBatchAirtimeRequest : IUserInfoRequest
    {
        public string BatchCode { get; set; }
        public string ProviderCode { get; set; }
        public decimal Airtime  { get; set; }
        public decimal Amount { get; set; }
        public float Discount { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public string ModifiedAccount { get; set; }


        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime-import", "DELETE")]
    public class DeleteBatchAirtimeRequest : IUserInfoRequest
    {
        public string BatchCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/airtime-import", "GET")]
    public class GetBatchAirtimeRequest : IUserInfoRequest
    {
        public string BatchCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }



}
