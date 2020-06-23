using Gnu.Framework.Core;
using RasmiOnline.Domain.Properties;

namespace RasmiOnline.Domain.Enum
{
    public enum TranslateType : byte
    {
        [LocalizeDescription(nameof(DisplayName.Official), typeof(DisplayName))]
        Official = 0,
        [LocalizeDescription(nameof(DisplayName.OfficialWithConfirms), typeof(DisplayName))]
        OfficialWithConfirms = 1,
        [LocalizeDescription(nameof(DisplayName.NonOfficial), typeof(DisplayName))]
        NonOfficial = 2
    }
}
