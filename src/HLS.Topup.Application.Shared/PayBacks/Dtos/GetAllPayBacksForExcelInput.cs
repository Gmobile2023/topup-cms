using System;

namespace HLS.Topup.PayBacks.Dtos
{
    public class GetAllPayBacksForExcelInput
    {
        public DateTime? FromTimeFilter { get; set; }
        
        public DateTime? ToTimeFilter { get; set; }
        
        public string CodeFilter { get; set; }
        
        public string NameFilter { get; set; }
        
        public int? StatusFilter { get; set; }
    }

    public class GetDetailPayBacksForExcelInput
    {
        public int PayBacksId { get; set; }
    }
}