using RasmiOnline.Domain.Properties;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class SurveySearchFilter : FilterBaseModel
    {
        [Display(Name = nameof(DisplayName.Subject), ResourceType = typeof(DisplayName))]
        public string Subject { get; set; }
    }
}
