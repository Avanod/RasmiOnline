using System.Data.Entity;
using Gnu.Framework.Core;
using RasmiOnline.Domain.Entity;
using Gnu.Framework.EntityFramework;
using RasmiOnline.Business.Properties;
using RasmiOnline.Business.Protocol;
using Gnu.Framework.EntityFramework.DataAccess;

namespace RasmiOnline.Business.Implement
{
    public class SurveyOptionBusiness : ISurveyOptionBusiness
    {
        readonly IUnitOfWork _uow;
        readonly IDbSet<SurveyOption> _survey;
        public SurveyOptionBusiness(IUnitOfWork uow)
        {
            _uow = uow;
            _survey = _uow.Set<SurveyOption>();
        }

        public IActionResponse<SurveyOption> Add(SurveyOption surveyOpt)
        {
            _survey.Add(surveyOpt);
            var rep = _uow.SaveChanges();
            return new ActionResponse<SurveyOption>
            {
                Result = surveyOpt,
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                IsSuccessful = rep.ToSaveChangeResult()
            };
        }

        public IActionResponse<bool> Delete(int id)
        {
            var opt = _survey.Find(id);
            _survey.Remove(opt);
            var rep = _uow.SaveChanges();
            return new ActionResponse<bool>
            {
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                IsSuccessful = rep.ToSaveChangeResult()
            };
        }
    }
}
