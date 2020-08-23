using RasmiOnline.Domain.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class SurveyResult
    {
        [Display(Name = nameof(DisplayName.Subject), ResourceType = typeof(DisplayName))]
        public string Subject { get; set; }

        [Display(Name = nameof(DisplayName.Text), ResourceType = typeof(DisplayName))]
        public string Text { get; set; }
        public List<SurveyResultSPModel> Items { get; set; }
    }
}
