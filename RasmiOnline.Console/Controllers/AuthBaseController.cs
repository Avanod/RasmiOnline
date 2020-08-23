using Gnu.Framework.Core;
using Gnu.Framework.Core.Authentication;
using RasmiOnline.Business.Protocol;
using RasmiOnline.Console.Properties;
using RasmiOnline.Domain.Entity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RasmiOnline.Console.Controllers
{
    public partial class AuthBaseController : Controller
    {
        readonly IUserBusiness _userSrv;
        public AuthBaseController(IUserBusiness userSrv)
        {
            _userSrv = userSrv;
        }

        public AuthBaseController() { }

        [NonAction]
        protected ActionResponse<string> SignIn(User user, bool rememberMe)
        {
            var menuRep = _userSrv.GetAvailableActions(user.UserId);
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
    }
}