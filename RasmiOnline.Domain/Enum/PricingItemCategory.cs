namespace RasmiOnline.Domain.Enum
{
    using System.ComponentModel;

    public enum PricingItemCategory : byte
    {
        [Description("مدارک تحصیلی")]
        EducationalDocuments = 1,

        [Description("مدارک شناسایی")]
        PersonalDocuments = 2,

        // [Description("اسناد قراردادی")]
        // ContractDocuments = 3,

        [Description("حکم و جواز")]
        JudicialDocuments = 4,

        [Description("اسناد مالکیت")]
        OwnershipDocuments = 5,

        [Description("سند و وکالتنامه")]
        TradingDocuments = 6,

        [Description("پروانه")]
        License = 7,

        [Description("گواهی")]
        Certification = 8,

        [Description("مدارک اشتغال به کار و شرکت ها")]
        CorporateDocuments = 9,

        [Description("متفرقه")]
        Other = 10
    }
}
