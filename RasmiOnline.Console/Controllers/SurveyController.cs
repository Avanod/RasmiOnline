using System.Web.Mvc;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using RasmiOnline.Business.Protocol;
using Gnu.Framework.Core;
using RasmiOnline.Console.Properties;
using System.Linq;

namespace RasmiOnline.Console.Controllers
{
    public partial class SurveyController : Controller
    {
        private readonly ISurveyBusiness _surveyBusiness;
        private readonly ISurveyOptionBusiness _surveyOptionBusiness;
        public SurveyController(ISurveyBusiness surveyBusiness, ISurveyOptionBusiness surveyOptionBusiness)
        {
            _surveyBusiness = surveyBusiness;
            _surveyOptionBusiness = surveyOptionBusiness;
        }
        public virtual ActionResult Search(SurveySearchFilter filter)
        {
            ViewBag.AutoSubmit = false;
            var result = _surveyBusiness.Get(filter);
            if (!Request.IsAjaxRequest()) return View(result);

            return PartialView(MVC.Survey.Views.Partials._SearchList, result.Result);
        }

        [HttpGet]
        public virtual PartialViewResult Add()
        {

            return PartialView(MVC.Survey.Views.Partials._Form, new ActionResponse<Survey> { IsSuccessful = true, Result = new Survey() });
        }

        [HttpPost]
        public virtual JsonResult Add(Survey model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = LocalMessage.ValidationFailed });
            return Json(_surveyBusiness.Add(model));
        }

        [HttpGet]
        public virtual PartialViewResult Update(int id)
        {

            var survey = _surveyBusiness.Find(id);
            //if (survey == null)
            //    return View(MVC.Shared.Views.Error);
            return PartialView(MVC.Survey.Views.Partials._Form, survey);
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

        [HttpGet]
        public virtual JsonResult Delete(int id = default(int))
            => Json(_surveyBusiness.Delete(id), JsonRequestBehavior.AllowGet);

        [HttpPost]
        public virtual JsonResult DeleteOption(int id = default(int))
             => Json(_surveyOptionBusiness.Delete(id), JsonRequestBehavior.AllowGet);

        [HttpGet, AllowAnonymous]
        public virtual ViewResult Index(int id)
        {
            return View(_surveyBusiness.Find(id));
        }

        [HttpPost]
        public virtual ViewResult Submit(int surveyId, int surveyOptionId)
        {
            return View(_surveyOptionBusiness.Add(new SurveyOption
            {
                SurveyId = surveyId,
                SelectedOption = surveyOptionId
            }));
        }

        [HttpGet]
        public virtual PartialViewResult ShowResult(int id)
        {
            var survey = _surveyBusiness.Find(id);
            return PartialView(MVC.Survey.Views.Partials._ShowResult, new SurveyResult { 
                Subject = survey.Result.Subject,
                Text = survey.Result.Text,
                Items = _surveyBusiness.GetResult(id)
            });
        }
    }
}