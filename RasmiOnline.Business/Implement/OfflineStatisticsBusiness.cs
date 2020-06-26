using System.Linq;
using Gnu.Framework.Core;
using System.Data.Entity;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Enum;
using RasmiOnline.Domain.Entity;
using RasmiOnline.Business.Protocol;
using Gnu.Framework.EntityFramework.DataAccess;
using System.Collections.Generic;

namespace RasmiOnline.Business.Implement
{
    public class OfflineStatisticsBusiness : IOfflineStatisticsBusiness
    {
        private readonly IDbSet<OfflineStatistics> _statistic;
        private readonly IUnitOfWork _uow;
        public OfflineStatisticsBusiness(IUnitOfWork uow)
        {
            _uow = uow;
            _statistic = uow.Set<OfflineStatistics>();
        }
        public StatisticModel Get()
        {
            var model = new StatisticModel();
            var sumItems = (from element in _statistic
                            where element.Type == StaticticsType.AllOrder
                                  || element.Type == StaticticsType.AllPayment
                                  || element.Type == StaticticsType.AllUser
                            orderby element.InsertDateMi
                            select element).Take(20).ToList();
            var userCount = sumItems.Where(x => x.Type == StaticticsType.AllUser).OrderByDescending(x => x.InsertDateMi).FirstOrDefault();
            model.UserCount = userCount == null ? 0 : userCount.Value;
            var orderCount = sumItems.Where(x => x.Type == StaticticsType.AllOrder).OrderByDescending(x => x.InsertDateMi).FirstOrDefault();
            model.OrderCount = orderCount == null ? 0 : orderCount.Value;
            var payCount = sumItems.Where(x => x.Type == StaticticsType.AllPayment).OrderByDescending(x => x.InsertDateMi).FirstOrDefault();
            model.PayCount = payCount == null ? 0 : payCount.Value;
            model.PayAmount = payCount == null ? 0 : payCount.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price;
            var items = _statistic.Where(x => x.Type == StaticticsType.OrderLastDays
            || x.Type == StaticticsType.PaymentLastDays
            || x.Type == StaticticsType.UserLastDays)
                .OrderByDescending(x => x.InsertDateMi)
                .Take(20).ToList();
            model.OrderInDays = items.Where(x => x.Type == StaticticsType.OrderLastDays)
                .Select(x => new
                {
                    Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                    x.Value
                })
                .OrderBy(x => x.Key)
               .ToDictionary(x => x.Key, x => x.Value);
            model.UserInDays = items.Where(x => x.Type == StaticticsType.UserLastDays)
                .Select(x => new
                {
                    Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].RegisterDateSh),
                    x.Value
                })
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, v => v.Value);
            //model.PayCounts = items.Where(x => x.Type == StaticticsType.PaymentLastDays)
            //    .ToDictionary(k => (string)(k.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh), v => v.Value);
            //model.PayAmounts = items.Where(x => x.Type == StaticticsType.PaymentLastDays)
            //    .ToDictionary(k => (string)(k.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh), v => v.Value);

            return model;
        }
    }
}
