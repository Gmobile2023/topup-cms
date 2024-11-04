using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLS.Topup.PayBacks.Dtos
{
    public class GetAllPayBacksInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromTimeFilter { get; set; }
        
        public DateTime? ToTimeFilter { get; set; }
        
        public string CodeFilter { get; set; }
        
        public string NameFilter { get; set; }
        
        public int? StatusFilter { get; set; }
    }
    
    public class GetPayBacksDetailTableInput : PagedAndSortedResultRequestDto
    {
        public int PayBacksId { get; set; }
        public List<int?> UserIds { get; set; }
    }
}