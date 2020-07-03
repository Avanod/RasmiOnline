using Gnu.Framework.Core;
using Gnu.Framework.EntityFramework;
using Gnu.Framework.EntityFramework.DataAccess;
using RasmiOnline.Business.Properties;
using RasmiOnline.Business.Protocol;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RasmiOnline.Business.Implement
{
    public class SurveyBusiness : ISurveyBusiness
    {
        readonly IUnitOfWork _uow;
        readonly IDbSet<Survey> _survey;
        public SurveyBusiness(IUnitOfWork uow)
        {
            _uow = uow;
            _survey = _uow.Set<Survey>();
        }

        public IActionResponse<Survey> Add(Survey survey)
        {
            _survey.Add(survey);
            var rep = _uow.SaveChanges();
            survey.SurveyOptions = null;
            return new ActionResponse<Survey>
            {
                Result = survey,
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                IsSuccessful = rep.ToSaveChangeResult()
            };
        }

        public IActionResponse<Survey> Delete(int id)
        {
            var item = _survey.Find(id);
            if (item == null) return new ActionResponse<Survey> { IsSuccessful = false, Message = BusinessMessage.RecordNotFound };
            _survey.Remove(item);
            var rep = _uow.SaveChanges();
            return new ActionResponse<Survey>
            {
                IsSuccessful = rep.ToSaveChangeResult(),
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                Result = item
            };
        }

        public IActionResponse<Survey> Find(int id)
        {
            var item = _survey.FirstOrDefault(x => x.SurveyId == id);
            if (item != null)
                item.SurveyOptions = _uow.Set<SurveyOption>().Where(x => x.SurveyId == id && x.SelectedOption == 0).ToList();
            return new ActionResponse<Survey>
            {
                IsSuccessful = item != null,
                Message = item == null ? BusinessMessage.RecordNotFound : null,
                Result = item
            };
        }

        public IActionResponse<Survey> Update(Survey survey)
        {
            var item = _survey.Find(survey.SurveyId);
            item.Subject = survey.Subject;
            item.Text = survey.Text;
            //_uow.Entry(survey).State = EntityState.Modified;
            if (survey.SurveyOptions != null)
                foreach (var opt in survey.SurveyOptions.Where(x => x.SurveyOptionId == 0).ToList())
                {
                    opt.SurveyId = survey.SurveyId;
                    _uow.Set<SurveyOption>().Add(opt);
                }

            var rep = _uow.SaveChanges();

            return new ActionResponse<Survey>
            {
                IsSuccessful = rep.ToSaveChangeResult(),
                Result = survey,
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error)
            };
        }

        public IActionResponse<List<Survey>> Get(SurveySearchFilter filterModel)
        {
            var response = new ActionResponse<List<Survey>>();
            var q = _survey.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterModel.Subject))
                q = q.Where(x => x.Subject.Contains(filterModel.Subject));
            response.Result = q.OrderBy(x => x.SurveyId)
                         .Select(s => s)
                         .Take(filterModel.ItemsCount)
                         .ToList();
            return response;
        }
    }
}
