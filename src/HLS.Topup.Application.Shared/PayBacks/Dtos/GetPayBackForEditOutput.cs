using System;

namespace HLS.Topup.PayBacks.Dtos
{
    public class GetPayBackForEditOutput
    {
        public CreateOrEditPayBacksDto PayBacks { get; set; }
        
        public string UserName { get; set; }
        
        public string UserApproved { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}