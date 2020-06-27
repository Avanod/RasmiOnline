using RasmiOnline.Business.Protocol;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using System.Web.Mvc;

namespace RasmiOnline.Console.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ISurveyBusiness _surveyBusiness;
        public SurveyController(ISurveyBusiness surveyBusiness)
        {
            surveyBusiness = _surveyBusiness;
        }
        public virtual ActionResult Manage(SurveySearchFilter filter)
        {
            return View(_surveyBusiness.Get(filter));
        }

        [HttpGet]
        public virtual ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public virtual JsonResult Add(Survey model)
        {
            return Json(_surveyBusiness.Add(model));
        }

        [HttpGet]
        public virtual ViewResult Update(int id)
        {

            var survey = _surveyBusiness.Find(id);
            if (survey == null)
                return View(MVC.Shared.Views.Error);
            return View(survey);
        }

        [HttpPost]
        public virtual JsonResult Update(Survey model)
        {
            var rep = _surveyBusiness.Update(model);
            if (!rep.IsSuccessful) return Json(new { IsSuccessful = false, rep.Message });
            return Json(new
            {
                IsSuccessful = true,
                rep.Message
            });
        }

    }
}