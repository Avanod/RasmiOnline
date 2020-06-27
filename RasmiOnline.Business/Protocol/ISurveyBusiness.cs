using Gnu.Framework.Core;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;

namespace RasmiOnline.Business.Protocol
{
    public interface ISurveyBusiness
    {
        IActionResponse<Survey> Add(Survey survey);
        IActionResponse<Survey> Delete(int id);
        IActionResponse<Survey> Find(int id);
        IActionResponse<Survey> Update(Survey survey);
        IActionResponse<List<Survey>> Get(SurveySearchFilter filterModel);
    }
}