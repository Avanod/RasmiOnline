using System.Linq;
using System.Data.Entity;
using Gnu.Framework.Core;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;
using Gnu.Framework.EntityFramework;
using RasmiOnline.Business.Properties;
using Gnu.Framework.EntityFramework.DataAccess;
using System;
using Gnu.Framework.AspNet.Cache;
using RasmiOnline.Business.Protocol;

namespace RasmiOnline.Business.Implement
{
    public class SmsTemplateBusiness : ISmsTemplateBusiness
    {
        readonly IUnitOfWork _uow;
        readonly IDbSet<SmsTemplate> _SmsTemplate;
        readonly ICacheProvider _cache;
        public SmsTemplateBusiness(IUnitOfWork uow, ICacheProvider cache)
        {
            _uow = uow;
            _SmsTemplate = _uow.Set<SmsTemplate>();
            _cache = cache;
        }

        public IActionResponse<SmsTemplate> Add(SmsTemplate SmsTemplate)
        {
            _SmsTemplate.Add(SmsTemplate);
            var rep = _uow.SaveChanges();
            return new ActionResponse<SmsTemplate>
            {
                Result = SmsTemplate,
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                IsSuccessful = rep.ToSaveChangeResult()
            };
        }

        public IActionResponse<SmsTemplate> Delete(int id)
        {
            var item = _SmsTemplate.Find(id);
            if (item == null) return new ActionResponse<SmsTemplate> { IsSuccessful = false, Message = BusinessMessage.RecordNotFound };
            _SmsTemplate.Remove(item);
            var rep = _uow.SaveChanges();
            return new ActionResponse<SmsTemplate>
            {
                IsSuccessful = rep.ToSaveChangeResult(),
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error),
                Result = item
            };
        }

        public IActionResponse<SmsTemplate> Find(int id)
        {
            var item = _SmsTemplate.FirstOrDefault(x => x.SmsTemplateId == id && !x.IsDeleted);

            return new ActionResponse<SmsTemplate>
            {
                IsSuccessful = item != null,
                Message = item == null ? BusinessMessage.RecordNotFound : null,
                Result = item
            };
        }

        public IActionResponse<SmsTemplate> Update(SmsTemplate SmsTemplate)
        {
            var item = _SmsTemplate.Find(SmsTemplate.SmsTemplateId);
            item.Key = SmsTemplate.Key;
            item.Title = SmsTemplate.Title;
            item.Text = SmsTemplate.Text;
            var rep = _uow.SaveChanges();

            return new ActionResponse<SmsTemplate>
            {
                IsSuccessful = rep.ToSaveChangeResult(),
                Result = SmsTemplate,
                Message = rep.ToSaveChangeMessageResult(BusinessMessage.Success, BusinessMessage.Error)
            };
        }

        public IActionResponse<List<SmsTemplate>> Get(SmsTemplateSearchFilter filterModel)
        {
            var response = new ActionResponse<List<SmsTemplate>>();
            var q = _SmsTemplate.AsNoTracking().AsQueryable().Where(x => !x.IsDeleted);
            if (filterModel.Key != null)
                q = q.Where(x => x.Key == filterModel.Key.ToString());
            if (!string.IsNullOrWhiteSpace(filterModel.Text))
                q = q.Where(x => x.Text.Contains(filterModel.Text));
            response.Result = q.OrderByDescending(x => x.SmsTemplateId)
                         .Select(s => s)
                         .Take(filterModel.ItemsCount)
                         .ToList();
            return response;
        }

        public List<SmsTemplate> GetAll()
        {
            const string key = "SmsTemplates";
            var items = (List<SmsTemplate>)_cache.GetItem(key);
            if (items != null) return items;
            items = _SmsTemplate.AsNoTracking().Where(x => !x.IsDeleted).ToList();
            _cache.PutItem(key, items, null, DateTime.Now.AddHours(2));
            return items;
        }
    }
}
