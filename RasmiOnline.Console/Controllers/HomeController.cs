using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Gnu.Framework.Core;
using System.Web.Security;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Enum;
using Gnu.Framework.AspNet.Mvc;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;
using RasmiOnline.Business.Protocol;
using RasmiOnline.Console.Properties;
using Gnu.Framework.Core.Authentication;
using RasmiOnline.Console.PaymentStrategy;
using Gnu.Framework.Core.Log;

namespace RasmiOnline.Console.Controllers
{
    [AllowAnonymous]
    public partial class HomeController : AuthBaseController
    {
        readonly IUserBusiness _userSrv;
        readonly IOrderBusiness _orderSrv;
        readonly IAttachmentBusiness _attachmentSrv;
        readonly IUserBusiness _userBusiness;
        readonly IPaymentGatewayBusiness _paymentGatewayBusiness;
        readonly IOrderBusiness _orderBusiness;
        readonly IAddressBusiness _addressBusiness;
        readonly Lazy<IUserInRoleBusiness> _userInRoleBusiness;
        readonly Lazy<ITransactionBusiness> _transBusiness;
        readonly Lazy<IPricingItemBusiness> _pricingItemBusiness;
        public HomeController(IUserBusiness userSrv,
            IOrderBusiness orderSrv,
            IAttachmentBusiness attachmentSrv,
            IUserBusiness userBusiness,
            IPaymentGatewayBusiness paymentGatewayBusiness,
            IOrderBusiness orderBusiness,
            IAddressBusiness addressBusiness,
            Lazy<IUserInRoleBusiness> userInRoleBusiness,
            Lazy<ITransactionBusiness> transBusiness,
            Lazy<IPricingItemBusiness> pricingItemBusiness) : base(userSrv)
        {
            _userSrv = userSrv;
            _orderSrv = orderSrv;
            _attachmentSrv = attachmentSrv;
            _userBusiness = userBusiness;
            _paymentGatewayBusiness = paymentGatewayBusiness;
            _orderBusiness = orderBusiness;
            _addressBusiness = addressBusiness;
            _userInRoleBusiness = userInRoleBusiness;
            _transBusiness = transBusiness;
            _pricingItemBusiness = pricingItemBusiness;
        }


        [NonAction]
        private (int price, int payedPrice) CheckPrice(Order order)
        {
            int price = order.TotalPrice();
            int payedPrice = _transBusiness.Value.GetTotalPayedPrice(order.OrderId);
            if (payedPrice > 0)
                price -= payedPrice;
            else if(!order.IsFullPayed)
                price = price / 2;
            return (price, payedPrice);
        }

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ViewResult AddOrder()
        {
            ViewBag.TranslateTypes = EnumConvertor.GetEnumElements<TranslateType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Description,
                    Value = x.Name
                }).ToList();
            return View(new AddOrderModel());
        }

        [HttpPost, AllowAnonymous]
        public virtual JsonResult AddOrder(AddOrderModel model, List<HttpPostedFileBase> attachments)
        {
            var addUser = _userSrv.Insert(model);
            if (!addUser.IsSuccessful) return Json(addUser);

            model.UserId = addUser.Result;
            var userInRole = new UserInRole
            {
                UserId = addUser.Result,
                RoleId = int.Parse(AppSettings.EndUserRoleId),
                IsActive = true,
                ExpireDateSh = PersianDateTime.Now.AddYears(5).ToString(PersianDateTimeFormat.Date)
            };

            if (!_userInRoleBusiness.Value.CheckExist(userInRole))
                _userInRoleBusiness.Value.Insert(userInRole);

            model.Status = OrderStatus.WaitForPricing;
            model.DayToDeliver = byte.Parse(AppSettings.DefaultDayToDeliver);
            var addOrder = _orderSrv.Add(model);
            if (!addOrder.IsSuccessful) return Json(addUser);

            var addFiles = _attachmentSrv.Insert(addOrder.Result, AttachmentType.OrderFiles, attachments);
            addOrder.Result.User = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                MobileNumber = long.Parse(model.MobileNumber)
            };

            return Json(new
            {
                addFiles.IsSuccessful,
                addFiles.Message,
                Result = MvcExtention.RenderViewToString(this, MVC.Home.Views.Partials._AfterAdd, addOrder.Result)
            });
        }

        [HttpGet, AllowAnonymous, Route("Home/{orderId:int}/{userId:Guid}")]
        public virtual ActionResult Payment(int orderId, Guid userId)
        {
            var user = _userBusiness.Find(userId);
            ViewBag.PaymentGatewayId = int.Parse(AppSettings.DefaultPaymentGatewayId);//_paymentGatewayBusiness.GetAll().First().PaymentGatewayId;
            if (user == null) return View(MVC.Order.Views.NotFound);
            var rep = SignIn(user, true);
            if (!rep.IsSuccessful) return View(MVC.Shared.Views.Error, (object)rep.Message);
            var order = _orderBusiness.Find(orderId, "OrderItems,Transactions,User,Address");
            if (order == null || order.UserId != userId)
                return View(MVC.Order.Views.NotFound);
            var priceCheck = CheckPrice(order);
            ViewBag.PayedPrice = priceCheck.payedPrice;
            ViewBag.CompletePayment = priceCheck.payedPrice > 0;
            ViewBag.Price = priceCheck.price;
            ViewBag.Addresses = _addressBusiness.GetAll(userId);
            ViewBag.Warnings = _pricingItemBusiness.Value.GetDescriptions(order.OrderItems?.Select(x=>x.PricingItemId).ToList());
            return View(order);
        }

        [HttpPost, AllowAnonymous]
        public virtual ActionResult Submit(CompleteOrderModel model)
        {
            try
            {
                #region Checking
                var gatewayRep = _paymentGatewayBusiness.GetPaymentGateway(model.PaymentGatewayId);
                if (!gatewayRep.IsSuccessful)
                    return Json(new { IsSuccessful = false, Message = LocalMessage.RecordsNotFound });
                if (model.AddressId != null)
                {
                    var addrRep = _addressBusiness.Find((User as ICurrentUserPrincipal).UserId, model.AddressId ?? 0);
                    if (!addrRep.IsSuccessful)
                        return Json(new { IsSuccessful = false, Message = LocalMessage.RecordsNotFound });
                }
                #endregion
                #region Fill Some Props of Model
                model.UserId = (User as ICurrentUserPrincipal).UserId;
                #endregion
                var findOrder = _orderBusiness.UpdateBeforePayment(model);
                if (!findOrder.IsSuccessful) return Json(findOrder);
                var result = PaymentFactory.GetInstance(gatewayRep.Result.BankName).Do(gatewayRep.Result, new TransactionModel
                {
                    OrderId = model.OrderId,
                    PaymentGatewayId = model.PaymentGatewayId,
                    Price = findOrder.Result.Item2,
                    UserId = (User as ICurrentUserPrincipal).UserId
                });
                return Json(result);
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return Json(new { IsSuccessful = false, Message = LocalMessage.Error });
            }

        }

        [HttpGet]
        public virtual ViewResult Error() => View();
    }
}