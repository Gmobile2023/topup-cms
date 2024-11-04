using Abp.Application.Services.Dto;
using System;
using HLS.Topup.Common;

namespace HLS.Topup.Sale.Dtos
{
    public class GetAllSaleMansInput : PagedAndSortedResultRequestDto
    {
        public string UserNameFilter { get; set; }

        public string PhoneNumberFilter { get; set; }

        public int? SaleLeaderFilter { get; set; }

        public DateTime? FromDateFilter { get; set; }

        public DateTime? ToDateFilter { get; set; }
        
        public string FullNameFilter { get; set; }
        
        public CommonConst.SystemAccountType? SaleTypeFilter { get; set; }
        
        public bool? Status { get; set; }
    }
}
