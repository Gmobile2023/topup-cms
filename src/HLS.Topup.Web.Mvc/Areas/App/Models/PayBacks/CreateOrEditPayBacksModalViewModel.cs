using System;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.PayBacks
{
    public class CreateOrEditPayBacksModalViewModel
    {
        public CreateOrEditPayBacksDto PayBacks { get; set; }
        
        public string UserName { get; set; }
        
        public string UserApproved { get; set; }
        
        public DateTime CreationTime { get; set; }

        public bool IsEditMode => PayBacks.Id.HasValue;
        
        public bool IsViewMode { get; set; }
    }
}