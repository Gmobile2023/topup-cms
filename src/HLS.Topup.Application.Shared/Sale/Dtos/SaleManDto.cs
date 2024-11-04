using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Sale.Dtos
{
    public class SaleManDto : UserInfoDto
    {
        public string SaleLeadName { get; set; }
        public long? CreatorUserId { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }
    }
}
