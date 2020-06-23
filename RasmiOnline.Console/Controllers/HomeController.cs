using Gnu.Framework.Core;
using RasmiOnline.Business.Protocol;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using RasmiOnline.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RasmiOnline.Console.Controllers
{
    [AllowAnonymous]
    public partial class HomeController : Controller
    {
        readonly IUserBusiness _userSrv;
        readonly IOrderBusiness _orderSrv;
        readonly IAttachmentBusiness _attachmentSrv;
        public HomeController(IUserBusiness userSrv, IOrderBusiness orderSrv, IAttachmentBusiness attachmentSrv)
        {
            _userSrv = userSrv;
            _orderSrv = orderSrv;
            _attachmentSrv = attachmentSrv;
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

        [HttpPost]
        public virtual JsonResult AddOrder(AddOrderModel model, List<HttpPostedFileBase> attachments)
        {
            var addUser = _userSrv.Insert(model);
            if (!addUser.IsSuccessful) return Json(addUser);
            model.UserId = addUser.Result;
            var addOrder = _orderSrv.QuickInsert(model);
            if (!addOrder.IsSuccessful) return Json(addUser);
            var addFiles = _attachmentSrv.Insert(addOrder.Result, AttachmentType.OrderFiles, attachments);
            return Json(new { addFiles.IsSuccessful, addFiles.Message });
        }
    }
}