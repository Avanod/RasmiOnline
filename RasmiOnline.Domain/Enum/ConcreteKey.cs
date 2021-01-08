using System.ComponentModel;

namespace RasmiOnline.Domain.Enum
{
    public enum ConcreteKey
    {
        [Description("ثبت نام کاربر")]
        User_Register,
        [Description("افزوده شدن سفارش")]
        Order_Add,
        [Description("در انتظار پرداخت")]
        Order_Wait_For_Payment,
        [Description("تسویه نهایی")]
        Order_Pay_All_Factor,
        [Description("تایید پیش نویس")]
        Order_Submit_Draft,
        [Description("لغو سفارش")]
        Order_Cancel,
        [Description("اتمام سفارش")]
        Order_Done,
        [Description("تغییر وضعیت سفارش")]
        Order_Status_Changed,
        [Description("پرداخت موفق سفارش")]
        Success_Payment,
        [Description("تراکنش غیر آنلاین")]
        Offline_Payment,

        [Description("تحویل ترجمه")]
        DeliveryFiles,

        //[Description("افزودن تراکنش")]
        //Transaction_Add,
        //[Description("ثبت نام کاربر")]
        //User_Register,
        //[Description("افزوده شدن سفارش")]
        //Order_Add,
        //[Description("در انتظار پرداخت")]
        //Waiting_For_Payment,
        //[Description("تسویه نهایی")]
        //Pay_All_Factor,
        //[Description("تایید پیش نویس")]
        //Submit_Draft,
        //[Description("تراکنش غیر آنلاین")]
        //Offline_Transaction_Add,
        //[Description("انصراف سفارش")]
        //Cancel_Order,
        //[Description("اتمام سفارش")]
        //Done,
        //[Description("جابجایی اداره")]
        //Order_Moved_Office,
        //[Description("تغییر وضعیت سفارش")]
        //Change_OrderState,
        //[Description("بارگذاری فایل")]
        //Attachment_Add,
        //[Description("پیامک پرداخت شتری")]
        //Pay_First_PaymentPart
    }
}
