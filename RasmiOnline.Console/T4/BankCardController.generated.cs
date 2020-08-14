// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace RasmiOnline.Dashboard.Controllers
{
    public partial class BankCardController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected BankCardController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult FormSubmited()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.FormSubmited);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public BankCardController Actions { get { return MVC.BankCard; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "BankCard";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "BankCard";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Manage = "Manage";
            public readonly string ShowList = "ShowList";
            public readonly string GetForm = "GetForm";
            public readonly string FormSubmited = "FormSubmited";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Manage = "Manage";
            public const string ShowList = "ShowList";
            public const string GetForm = "GetForm";
            public const string FormSubmited = "FormSubmited";
        }


        static readonly ActionParamsClass_GetForm s_params_GetForm = new ActionParamsClass_GetForm();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetForm GetFormParams { get { return s_params_GetForm; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetForm
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_FormSubmited s_params_FormSubmited = new ActionParamsClass_FormSubmited();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FormSubmited FormSubmitedParams { get { return s_params_FormSubmited; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FormSubmited
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Manage = "Manage";
            }
            public readonly string Manage = "~/Views/BankCard/Manage.cshtml";
            static readonly _PartialsClass s_Partials = new _PartialsClass();
            public _PartialsClass Partials { get { return s_Partials; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _PartialsClass
            {
                static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
                public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
                public class _ViewNamesClass
                {
                    public readonly string _Form = "_Form";
                    public readonly string _List = "_List";
                }
                public readonly string _Form = "~/Views/BankCard/Partials/_Form.cshtml";
                public readonly string _List = "~/Views/BankCard/Partials/_List.cshtml";
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_BankCardController : RasmiOnline.Dashboard.Controllers.BankCardController
    {
        public T4MVC_BankCardController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ManageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Manage()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Manage);
            ManageOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ShowListOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo);

        [NonAction]
        public override System.Web.Mvc.PartialViewResult ShowList()
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.ShowList);
            ShowListOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GetFormOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetForm(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetForm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            GetFormOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void FormSubmitedOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, RasmiOnline.Domain.Entity.BankCard model);

        [NonAction]
        public override System.Web.Mvc.JsonResult FormSubmited(RasmiOnline.Domain.Entity.BankCard model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.FormSubmited);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            FormSubmitedOverride(callInfo, model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
