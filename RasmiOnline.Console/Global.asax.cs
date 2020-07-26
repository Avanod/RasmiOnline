namespace RasmiOnline.Console
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Web.Mvc;
    using Business.Protocol;
    using System.Web.Routing;
    using System.Web.Security;
    using Gnu.Framework.Core.Log;
    using DependencyResolver.Ioc;
    using System.Web.Optimization;
    using System.Web.Script.Serialization;
    using Gnu.Framework.Core.Authentication;
    using RasmiOnline.Console.QuartzService;

    public class MvcApplication : HttpApplication
    {
        private IUserBusiness _userBiz;

        private void RedirectToAuthentication()
        {
            FormsAuthentication.SignOut();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            Response.Redirect(urlHelper.Action(MVC.OAuth.ActionNames.Index, MVC.OAuth.Name));
        }

        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            IocInitializer.Initialize();
            //JobScheduler.Start();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new IocControllerFactory());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-option", "DENY");
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            try
            {
                if (authCookie != null)
                {
                    _userBiz = IocInitializer.GetInstance(typeof(IUserBusiness)) as Business.Implement.UserBusiness;
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var currentUser = serializer.Deserialize<CurrentUserPrincipal>(authTicket.UserData);
                    currentUser.SetIdentity(authTicket.Name);
                    var rep = _userBiz.GetAvailableActions(currentUser.UserId);
                    if (rep != null)
                    {
                        currentUser.UserActionList = rep.Items.ToList();
                        HttpContext.Current.User = currentUser;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLoger.Info(authCookie.Value, GlobalVariable.LogPath);
                FileLoger.Error(ex, GlobalVariable.LogPath);
                RedirectToAuthentication();
            }

        }
    }
}