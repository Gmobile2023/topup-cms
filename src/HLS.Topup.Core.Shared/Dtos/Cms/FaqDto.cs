using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HLS.Topup.Dtos.Cms
{
    public class FaqDto
    {
        [DataMember(Name = "question_type")] public string QuestionType { get; set; }
        [DataMember(Name = "group_content")] public List<QuestionContentDto> GroupContent { get; set; }
    }

    public class QuestionContentDto
    {
        [DataMember(Name = "title")] public string Title { get; set; }
        [DataMember(Name = "answer")] public string Answer { get; set; }
    }
    
    public class AcfFaqsAppDto
    {
        [DataMember(Name = "acf")] public AcfFaqsField Acf { get; set; }
    }

    public class AcfFaqsField
    {
        [DataMember(Name = "question")] public List<FaqDto> Question { get; set; }
    }
}