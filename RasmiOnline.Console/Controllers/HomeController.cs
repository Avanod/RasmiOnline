using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
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

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ViewResult AddOrder()
        {
            return View(new AddOrderModel());
        }

        [HttpPost]
        public virtual JsonResult AddOrder(AddOrderModel model)
        {
            return Json(new { });
        }
    }
}