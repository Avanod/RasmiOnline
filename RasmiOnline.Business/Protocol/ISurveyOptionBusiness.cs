using Gnu.Framework.Core;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;

namespace RasmiOnline.Business.Protocol
{
    public interface ISurveyOptionBusiness
    {
        IActionResponse<int> AddRange(List<SurveyOption> options);
        IActionResponse<SurveyOption> Add(SurveyOption survey);
        IActionResponse<bool> Delete(int id);
    }
}