namespace RasmiOnline.Console.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Business.Protocol;
    using Gnu.Framework.Core;
    using RasmiOnline.Domain.Dto;
    using RasmiOnline.Domain.Entity;
    using System.Collections.Generic;
    using RasmiOnline.Console.Properties;
    using RasmiOnline.Domain.Enum;

    public partial class SmsTemplateController : Controller
    {
        #region Contructor
        private readonly ISmsTemplateBusiness _SmsTemplateBusiness;

        public SmsTemplateController(ISmsTemplateBusiness SmsTemplateBusiness)
        {
            _SmsTemplateBusiness = SmsTemplateBusiness;
        }
        #endregion

        #region Private Methods

        [NonAction]
        private List<SelectListItem> GetTypes()
            => EnumConvertor.GetEnumElements<ConcreteKey>()
            .Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = x.Name
            }).ToList();
        #endregion

        [HttpGet]
        public virtual ActionResult Search(SmsTemplateSearchFilter model)
        {
            ViewBag.Types = GetTypes();
            var result = _SmsTemplateBusiness.Get(model);
            if (!Request.IsAjaxRequest()) return View(result);

            return PartialView(MVC.SmsTemplate.Views.Partials._SearchList, result.Result);
        }

        [HttpGet]
        public virtual PartialViewResult GetForm(int id = default(int))
        {
            ViewBag.Types = GetTypes();
            if (id > 0)
            {
                var findedSmsTemplate = _SmsTemplateBusiness.Find(id);
               
                return PartialView(MVC.SmsTemplate.Views.Partials._Form, findedSmsTemplate);
            }
            return PartialView(MVC.SmsTemplate.Views.Partials._Form, new ActionResponse<SmsTemplate> { IsSuccessful = true, Result = new SmsTemplate()});
        }

        [HttpPost]
        public virtual JsonResult FormSubmited(SmsTemplate model)
        {
            var response = new ActionResponse<bool>();
            if (model.IsNull() || !ModelState.IsValid)
            {
                response.Message = LocalMessage.InvalidFormData;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            IActionResponse<SmsTemplate> bizResult = model.SmsTemplateId <= 0 ? bizResult = _SmsTemplateBusiness.Add(model) : bizResult = _SmsTemplateBusiness.Update(model);
            response.Result = response.IsSuccessful = bizResult.IsSuccessful;
            response.Message = bizResult.Message;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult Delete(int id = default(int))
            => Json((id > 0 ? _SmsTemplateBusiness.Delete(id) : new ActionResponse<SmsTemplate> { Message = LocalMessage.BadRequest }), JsonRequestBehavior.AllowGet);
    }
}