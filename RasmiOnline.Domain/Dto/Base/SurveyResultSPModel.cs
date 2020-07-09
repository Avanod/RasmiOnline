using RasmiOnline.Domain.Properties;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class SurveyResultSPModel
    {
        public long Option { get; set; }
        [Display(Name = nameof(DisplayName.Option), ResourceType = typeof(DisplayName))]
        public string Text { get; set; }
        [Display(Name = nameof(DisplayName.Count), ResourceType = typeof(DisplayName))]
        public int Tedad { get; set; }
        [Display(Name = nameof(DisplayName.AnswerPercent), ResourceType = typeof(DisplayName))]
        public int Percent { get; set; }
    }
}
