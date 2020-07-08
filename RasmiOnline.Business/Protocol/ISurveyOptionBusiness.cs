using Gnu.Framework.Core;
using RasmiOnline.Domain.Entity;

namespace RasmiOnline.Business.Protocol
{
    public interface ISurveyOptionBusiness
    {
        IActionResponse<SurveyOption> Add(SurveyOption survey);
        IActionResponse<bool> Delete(int id);
    }
}