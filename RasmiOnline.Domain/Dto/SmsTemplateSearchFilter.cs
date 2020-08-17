using RasmiOnline.Domain.Enum;

namespace RasmiOnline.Domain.Dto
{
    public class SmsTemplateSearchFilter : FilterBaseModel
    {
        public ConcreteKey? Key { get; set; }

        public string Text { get; set; }
    }
}
