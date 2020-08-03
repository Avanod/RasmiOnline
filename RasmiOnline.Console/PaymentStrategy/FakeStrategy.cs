namespace RasmiOnline.Console.PaymentStrategy
{
    using System;
    using ZarinPal;
    using System.Net;
    using Properties;
    using System.Web;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Business.Protocol;
    using Gnu.Framework.Core;
    using Business.Observers;
    using DependencyResolver.Ioc;
    using Gnu.Framework.Core.Authentication;

    public class FakeStrategy : IPaymentStrategy
    {
        readonly PaymentGatewayImplementationServicePortTypeClient _zarinPal;
        readonly IUserBusiness _userBusiness;
        readonly ITransactionBusiness _transactionBusiness;
        readonly IOrderBusiness _orderBusiness;
        readonly Lazy<IObserverManager> _observerManager;
        public FakeStrategy()
        {
            _zarinPal = new PaymentGatewayImplementationServicePortTypeClient();
            _userBusiness = IocInitializer.GetInstance<IUserBusiness>();
            _transactionBusiness = IocInitializer.GetInstance<ITransactionBusiness>();
            _orderBusiness = IocInitializer.GetInstance<IOrderBusiness>();
            _observerManager = IocInitializer.GetInstance<Lazy<IObserverManager>>();
        }


        public IActionResponse<string> Do(PaymentGateway gateway, TransactionModel model)
        {
            throw new NotImplementedException();
        }

        public IActionResponse<string> Verify(PaymentGateway gateway, Transaction model, object responseGateway = null)
        {
            if (model.OrderId != 0)
                _orderBusiness.UpdateStatus(model.OrderId);
            model.IsSuccess = true;
            model.TrackingId = "123";
            model.Status = "1";
            _transactionBusiness.Update(model);

            _observerManager.Value.Notify(ConcreteKey.Transaction_Add, new ObserverMessage
            {
                SmsContent = string.Format(LocalMessage.Transaction_Add_Sms, (HttpContext.Current.User as ICurrentUserPrincipal).FullName, model.OrderId),
                BotContent = string.Format(LocalMessage.Transaction_Add_Bot, (HttpContext.Current.User as ICurrentUserPrincipal).FullName,
                                        model.OrderId, gateway.BankName.GetLocalizeDescription(),
                                        model.Price.ToString("0,0"),
                                        model.TrackingId),
                Key = nameof(Transaction),
                UserId = (HttpContext.Current.User as ICurrentUserPrincipal).UserId,
            });
            return new ActionResponse<string>
            {
                IsSuccessful = true,
                Message = "عملیات پرداخت با موفقیت انجام شد",
                Result = model.TrackingId.ToString()
            };
        }
    }
}