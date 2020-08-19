using Gnu.Framework.Core;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using RasmiOnline.Domain.Enum;
using System.Collections.Generic;

namespace RasmiOnline.Business.Protocol
{
    public interface ISmsTemplateBusiness
    {
        IActionResponse<SmsTemplate> Add(SmsTemplate SmsTemplate);
        IActionResponse<SmsTemplate> Delete(int id);
        IActionResponse<SmsTemplate> Find(int id);
        string Find(ConcreteKey key);
        IActionResponse<List<SmsTemplate>> Get(SmsTemplateSearchFilter filterModel);
        List<SmsTemplate> GetAll();
        IActionResponse<SmsTemplate> Update(SmsTemplate SmsTemplate);
    }
}