using System;
using HLS.Topup.LimitationManager.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using HLS.Topup.Services.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.LimitProducts
{
    public class CreateOrEditLimitProductModalViewModel
    {
        public CreateOrEditLimitProductDto LimitProduct { get; set; }

        public string UserName { get; set; }
        
        public List<LimitProductUserLookupTableDto> LimitProductUserList { get; set; }
        
        public List<LimitProductServiceLookupTableDto> LimitProductServiceList { get; set; }
        
        public bool IsEditMode => LimitProduct.Id.HasValue;
        
        public bool IsViewMode { get; set; }
        
        public string UserApproved { get; set; }

        public DateTime CreationTime { get; set; }
        
        public string AgentName { get; set; }
    }
}