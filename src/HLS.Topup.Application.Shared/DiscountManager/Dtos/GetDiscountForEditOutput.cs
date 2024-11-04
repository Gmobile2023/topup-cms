using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.DiscountManager.Dtos
{
    public class GetDiscountForEditOutput
    {
        public CreateOrEditDiscountDto Discount { get; set; }

        public string UserName { get; set; }

        public string CreationTime { get; set; }
        
        public string UserCreated { get; set; }
        public string UserApproved { get; set; }
    }
}