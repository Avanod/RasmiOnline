using System.ComponentModel;

namespace RasmiOnline.Domain.Enum
{
    public enum ConcreteKey
    {
        [Description("افزودن تراکنش")]
        Transaction_Add,
        [Description("ثبت نام کاربر")]
        User_Register,
        [Description("افزوده شدن سفارش")]
        Order_Add,
        [Description("در انتظار پرداخت")]
        Waiting_For_Payment,
        [Description("تسویه نهایی")]
        Pay_All_Factor,
        [Description("تایید پیش نویس")]
        Submit_Draft,
        [Description("تراکنش غیر آنلاین")]
        Offline_Transaction_Add,
        [Description("انصراف سفارش")]
        Cancel_Order,
        [Description("اتمام سفارش")]
        Done,
        [Description("جابجایی اداره")]
        Order_Moved_Office,
        [Description("تغییر وضعیت سفارش")]
        Change_OrderState,
        [Description("بارگذاری فایل")]
        Attachment_Add,
        [Description("پیامک پرداخت شتری")]
        Pay_First_PaymentPart
    }
}
