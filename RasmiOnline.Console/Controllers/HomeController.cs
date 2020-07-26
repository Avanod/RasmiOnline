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

namespace RasmiOnline.Console.Controllers
{
    [AllowAnonymous]
    public partial class HomeController : Controller
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
        public HomeController(IUserBusiness userSrv,
            IOrderBusiness orderSrv,
            IAttachmentBusiness attachmentSrv,
            IUserBusiness userBusiness,
            IPaymentGatewayBusiness paymentGatewayBusiness,
            IOrderBusiness orderBusiness,
            IAddressBusiness addressBusiness,
            Lazy<IUserInRoleBusiness> userInRoleBusiness,
            Lazy<ITransactionBusiness> transBusiness)
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
        }

        [NonAction]
        private ActionResponse<string> SignIn(User user, bool rememberMe)
        {
            var menuRep = _userBusiness.GetAvailableActions(user.UserId);
            if (menuRep == null)
                return new ActionResponse<string>
                {
                    IsSuccessful = false,
                    Message = LocalMessage.ThereIsNoView
                };

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                HttpCookie _AuthCookie = new HttpCookie($"_{FormsAuthentication.FormsCookieName}", (User as ICurrentUserPrincipal).UserId.ToString())
                {
                    Expires = authCookie.Expires
                };
                HttpContext.Response.Cookies.Add(_AuthCookie);
            }

            var currentUser = new CurrentUserPrincipal();
            currentUser.UserId = user.UserId;
            currentUser.FullName = $"{user.FirstName} {user.LastName}";
            currentUser.UserName = user.Email.ToString();
            currentUser.CustomField = new UserExtraData { MobileNumber = user.MobileNumber };
            var expDateTime = rememberMe ? DateTime.Now.AddHours(int.Parse(AppSettings.AuthTimeoutWithRemeberMeInHours)) : DateTime.Now.AddMinutes(int.Parse(AppSettings.AuthTimeoutInMiutes));
            string userData = currentUser.SerializeToJson();
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.MobileNumber.ToString(), DateTime.Now, expDateTime, true, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
            {
                Expires = expDateTime,
                HttpOnly = true
            };
            //FormsAuthentication.set
            //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Response.Cookies.Add(cookie);
            //var currentUser = serializer.Deserialize<CurrentUserPrincipal>(authTicket.UserData);
            currentUser.SetIdentity(authTicket.Name);
            currentUser.UserActionList = menuRep.Items.ToList();
            System.Web.HttpContext.Current.User = currentUser;
            if (menuRep.DefaultUserAction.RoleId != int.Parse(AppSettings.UserRoleId))
                return new ActionResponse<string>
                {
                    IsSuccessful = true
                };

            return new ActionResponse<string>
            {
                IsSuccessful = true
            };

        }

        [NonAction]
        private int GetPrice(Order order)
        {
            int price = order.TotalPrice();
            int payedPrice = _transBusiness.Value.GetTotalPayedPrice(order.OrderId);
            if (!order.IsFullPayed)
                if (payedPrice == 0) price = price / 2;
                else price = price - payedPrice;
            return price;
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
            //ViewBag.OrderId = order.OrderId;
            //ViewBag.LangType = order.LangType;
            //LoadRelatedInfo(true, order.LangType);
            ViewBag.PayedPrice = _transBusiness.Value.GetTotalPayedPrice(orderId);
            ViewBag.CompletePayment = ViewBag.PayedPrice > 0;
            ViewBag.Price = GetPrice(order);
            ViewBag.Addresses = _addressBusiness.GetAll(userId);
            return View(order);
        }

        [HttpPost, AllowAnonymous]
        public virtual ActionResult Submit(CompleteOrderModel model)
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
            var findOrder = _orderBusiness.CompleteOrder(model);
            if (!findOrder.IsSuccessful) return Json(findOrder);
            //if (model.PaymentType == PaymentType.InPerson)
            //{
            //    return Json(new { rep.IsSuccessful, Result = Url.Action(MVC.Attachment.ActionNames.UploadAfterTransacttion, MVC.Attachment.Name, new { rep.Result.OrderId }) });
            //}

            var result = PaymentFactory.GetInstance(gatewayRep.Result.BankName).Do(gatewayRep.Result, new TransactionModel
            {
                OrderId = model.OrderId,
                PaymentGatewayId = model.PaymentGatewayId,
                Price = GetPrice(findOrder.Result),
                UserId = (User as ICurrentUserPrincipal).UserId
            });
            //TODO:Remove
            result.Result = AppSettings.BaseDomain + "/Transaction/FakeVerify?IN=" + result.Result;
            return Json(result);
        }
    }
}