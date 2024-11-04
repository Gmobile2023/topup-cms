using System.Collections.Generic;

namespace HLS.Topup.Web.Models.Faqs
{
    public class FaqsViewModel
    {
        public List<FaqsDto> Items { get; set; }
    }
    
    public class FaqsDto
    {
        public string QuestionType { get; set; }

        public List<QuestionItem> GroupContent { get; set; }
    }

    public class QuestionItem
    {
        public string Title { get; set; }

        public string Answer { get; set; }
    }
}