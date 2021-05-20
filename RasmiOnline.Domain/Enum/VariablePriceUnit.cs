using Gnu.Framework.Core;
using RasmiOnline.Domain.Properties;
using System.ComponentModel;

namespace RasmiOnline.Domain.Enum
{
    public enum VariablePriceUnit : byte
    {
        [LocalizeDescription(nameof(DisplayName.Vrb_Dars), typeof(DisplayName))]
        
        [Description("مدارک تحصیلی")]
        Vrb_Dars = 0,
        [LocalizeDescription(nameof(DisplayName.Vrb_Description), typeof(DisplayName))]
        Vrb_Description = 1,
        [LocalizeDescription(nameof(DisplayName.Vrb_Matn), typeof(DisplayName))]
        Vrb_Matn = 2,
        [LocalizeDescription(nameof(DisplayName.Vrb_Descriptions), typeof(DisplayName))]
        Vrb_Descriptions = 3,
        [LocalizeDescription(nameof(DisplayName.Vrb_HalfDescriptions), typeof(DisplayName))]
        Vrb_HalfDescriptions = 4,
        [LocalizeDescription(nameof(DisplayName.Vrb_Govahi), typeof(DisplayName))]
        Vrb_Govahi = 5,
        [LocalizeDescription(nameof(DisplayName.Vrb_TaghirMahal), typeof(DisplayName))]
        Vrb_TaghirMahal = 6,
        [LocalizeDescription(nameof(DisplayName.Vrb_RialiItem), typeof(DisplayName))]
        Vrb_RialiItem = 7,
        [LocalizeDescription(nameof(DisplayName.Vrb_Gharardad), typeof(DisplayName))]
        Vrb_Gharardad = 8,
        [LocalizeDescription(nameof(DisplayName.Vrb_Item), typeof(DisplayName))]
        Vrb_Item = 9,
        [LocalizeDescription(nameof(DisplayName.Vrb_Employee), typeof(DisplayName))]
        Vrb_Employee = 10,
        [LocalizeDescription(nameof(DisplayName.Vrb_Asasname), typeof(DisplayName))]
        Vrb_Asasname = 11,
        [LocalizeDescription(nameof(DisplayName.Vrb_Perferazh), typeof(DisplayName))]
        Vrb_Perferazh = 12,
        [LocalizeDescription(nameof(DisplayName.Vrb_Vekalat), typeof(DisplayName))]
        Vrb_Vekalat = 13,
        [LocalizeDescription(nameof(DisplayName.Vrb_Transaction), typeof(DisplayName))]
        Vrb_Transaction = 14,
        [LocalizeDescription(nameof(DisplayName.Vrb_Tazrigh), typeof(DisplayName))]
        Vrb_Tazrigh = 15
    }
}
