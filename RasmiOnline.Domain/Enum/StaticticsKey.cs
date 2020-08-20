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

        [LocalizeDescription(nameof(DisplayName.AllLicenseItemPayment), typeof(DisplayName))]
        AllLicenseItemPayment = 7,

        [LocalizeDescription(nameof(DisplayName.LicenseItemPaymentLastDays), typeof(DisplayName))]
        LicenseItemPaymentLastDays = 8,

        [LocalizeDescription(nameof(DisplayName.AllOtherItemPayment), typeof(DisplayName))]
        AllOtherItemPayment = 9,

        [LocalizeDescription(nameof(DisplayName.OtherItemPaymentLastDays), typeof(DisplayName))]
        OtherItemPaymentLastDays = 10,

        [LocalizeDescription(nameof(DisplayName.PaymentLastMonths), typeof(DisplayName))]
        PaymentLastMonths = 11,

        [LocalizeDescription(nameof(DisplayName.LicenseItemPaymentLastMonths), typeof(DisplayName))]
        LicenseItemPaymentLastMonths = 12,

        [LocalizeDescription(nameof(DisplayName.OtherItemPaymentLastMonths), typeof(DisplayName))]
        OtherItemPaymentLastMonths = 13,

    }
}