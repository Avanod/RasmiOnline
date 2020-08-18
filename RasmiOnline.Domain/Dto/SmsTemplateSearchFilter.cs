using RasmiOnline.Domain.Enum;
using RasmiOnline.Domain.Properties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class SmsTemplateSearchFilter : FilterBaseModel
    {
        [Display(Name = nameof(DisplayName.Key), ResourceType = typeof(DisplayName))]
        public ConcreteKey? Key { get; set; }

        [Display(Name = nameof(DisplayName.Text), ResourceType = typeof(DisplayName))]
        public string Text { get; set; }
    }
}
