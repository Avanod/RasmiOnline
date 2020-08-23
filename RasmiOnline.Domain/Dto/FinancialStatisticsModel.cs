using Gnu.Framework.Core;
using RasmiOnline.Domain.Properties;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class FinancialStatisticsModel
    {
        [Display(Name = nameof(DisplayName.AllPayment), ResourceType = typeof(DisplayName))]
        public int AllPayment { get; set; }//5

        [Display(Name = nameof(DisplayName.AllOtherItemPayment), ResourceType = typeof(DisplayName))]
        public int AllOtherItemPayment { get; set; }//9

        [Display(Name = nameof(DisplayName.AllLicenseItemPayment), ResourceType = typeof(DisplayName))]
        public int AllLicenseItemPayment { get; set; }//7

        [Display(Name = nameof(DisplayName.LicenseItemPaymentLastDays), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> LicenseItemPaymentLastDays { get; set; } = new Dictionary<string, int>();//8

        [Display(Name = nameof(DisplayName.PaymentLastDays), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> PaymentLastDays { get; set; } = new Dictionary<string, int>();//6

        [Display(Name = nameof(DisplayName.OtherItemPaymentLastDays), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> OtherItemPaymentLastDays { get; set; } = new Dictionary<string, int>();//10

        [Display(Name = nameof(DisplayName.PaymentLastMonths), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> PaymentLastMonths { get; set; } = new Dictionary<string, int>();//11

        [Display(Name = nameof(DisplayName.LicenseItemPaymentLastMonths), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> LicenseItemPaymentLastMonths { get; set; } = new Dictionary<string, int>();//12

        [Display(Name = nameof(DisplayName.OtherItemPaymentLastMonths), ResourceType = typeof(DisplayName))]
        public IDictionary<string, int> OtherItemPaymentLastMonths { get; set; } = new Dictionary<string, int>();//13
    }
}
