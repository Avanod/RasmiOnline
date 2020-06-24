namespace RasmiOnline.Domain.Enum
{
    using Domain.Properties;
    using Gnu.Framework.Core;

    public enum StaticticsType : byte
    {
        [LocalizeDescription(nameof(DisplayName.AllStatictics), typeof(DisplayName))]
        AllStatictics = 0,

        [LocalizeDescription(nameof(DisplayName.AllUser), typeof(DisplayName))]
        AllUser = 1,

        [LocalizeDescription(nameof(DisplayName.UserLastDays), typeof(DisplayName))]
        UserLastDays = 2,

        [LocalizeDescription(nameof(DisplayName.AllOrder), typeof(DisplayName))]
        AllOrder = 3,

        [LocalizeDescription(nameof(DisplayName.OrderLastDays), typeof(DisplayName))]
        OrderLastDays = 4,

        [LocalizeDescription(nameof(DisplayName.AllPayment), typeof(DisplayName))]
        AllPayment = 5,

        [LocalizeDescription(nameof(DisplayName.PaymentLastDays), typeof(DisplayName))]
        PaymentLastDays = 6,
    }
}