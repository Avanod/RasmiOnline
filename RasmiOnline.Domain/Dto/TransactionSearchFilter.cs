using System;
using RasmiOnline.Domain.Properties;
using System.ComponentModel.DataAnnotations;

namespace RasmiOnline.Domain.Dto
{
    public class TransactionSearchFilter : FilterBaseModel
    {
        [Display(Name = nameof(DisplayName.OrderId), ResourceType = typeof(DisplayName))]
        public int? OrderId { get; set; }

        [Display(Name = nameof(DisplayName.MobileNumber), ResourceType = typeof(DisplayName))]
        public string MobileNumber { get; set; }

        [Display(Name = nameof(DisplayName.PaymentGatewayId), ResourceType = typeof(DisplayName))]
        public int? PaymentGatewayId { get; set; }

        [Display(Name = nameof(DisplayName.BankCard), ResourceType = typeof(DisplayName))]
        public int? BankCardId { get; set; }

        [Display(Name = nameof(DisplayName.State), ResourceType = typeof(DisplayName))]
        public string Status { get; set; }

        [Display(Name = nameof(DisplayName.TrackingId), ResourceType = typeof(DisplayName))]
        public string TrackingId { get; set; }

        [Display(Name = nameof(DisplayName.FromDate), ResourceType = typeof(DisplayName))]
        public string FromDateSh { get; set; }

        [Display(Name = nameof(DisplayName.ToDate), ResourceType = typeof(DisplayName))]
        public string ToDateSh { get; set; }
    }
}
