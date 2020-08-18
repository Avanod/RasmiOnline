using System.ComponentModel;

namespace RasmiOnline.Domain.Enum
{
    public enum TranslateType : byte
    {
        [Description("رسمی با مهر مترجم")]
        OfficialWithTranslatorSign = 0,
        [Description("رسمی همراه با تاییدات دادگستری و وزارت امور خارجه")]
        OfficialWithConfirms = 1,
        [Description("ناتی(استرالیا و نیوزلند)")]
        Nati = 2,
        [Description("ناجیت(آمریکا و کانادا)")]
        Nagit = 3,
        [Description("غیر رسمی")]
        Others = 4
    }
}
