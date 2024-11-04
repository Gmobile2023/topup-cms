using System.Collections.Generic;

namespace HLS.Topup.Cms.Dots
{
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

    public class FaqsItems
    {
        public List<FaqsDto> Question { get; set; }
    }
}