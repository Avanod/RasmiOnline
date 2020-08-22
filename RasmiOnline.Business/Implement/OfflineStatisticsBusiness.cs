using System.Linq;
using Gnu.Framework.Core;
using System.Data.Entity;
using RasmiOnline.Domain.Dto;
using RasmiOnline.Domain.Enum;
using RasmiOnline.Domain.Entity;
using System.Collections.Generic;
using RasmiOnline.Business.Protocol;
using Gnu.Framework.EntityFramework.DataAccess;

namespace RasmiOnline.Business.Implement
{
    public class OfflineStatisticsBusiness : IOfflineStatisticsBusiness
    {
        private readonly IDbSet<OfflineStatistics> _statistic;
        private readonly IUnitOfWork _uow;
        private readonly int getCount = 26;
        public OfflineStatisticsBusiness(IUnitOfWork uow)
        {
            _uow = uow;
            _statistic = uow.Set<OfflineStatistics>();
        }
        public StatisticModel Get()
        {
            var model = new StatisticModel();
            var sumItems = _statistic.Where(x => x.Type == StaticticsType.AllOrder
                                  || x.Type == StaticticsType.AllPayment
                                  || x.Type == StaticticsType.AllUser)
                .OrderByDescending(x => x.InsertDateMi).Take(getCount).ToList();

            var userCount = sumItems.FirstOrDefault(x => x.Type == StaticticsType.AllUser);
            model.UserCount = userCount == null ? 0 : userCount.Value;
            var orderCount = sumItems.FirstOrDefault(x => x.Type == StaticticsType.AllOrder);
            model.OrderCount = orderCount == null ? 0 : orderCount.Value;
            var payCount = sumItems.FirstOrDefault(x => x.Type == StaticticsType.AllPayment);
            model.PayCount = payCount == null ? 0 : payCount.Value;
            model.PayAmount = payCount == null ? 0 : payCount.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price;
            var items = _statistic.Where(x => x.Type == StaticticsType.OrderLastDays
            || x.Type == StaticticsType.PaymentLastDays
            || x.Type == StaticticsType.UserLastDays)
                .OrderByDescending(x => x.InsertDateMi)
                .Take(getCount).ToList();
            foreach (var item in items.Where(x => x.Type == StaticticsType.OrderLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                x.Value
            })
                .OrderBy(x => x.Key).ToList())
            {
                if (!model.OrderInDays.Any(x => x.Key == item.Key))
                    model.OrderInDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.UserLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].RegisterDateSh),
                x.Value
            })
                .OrderBy(x => x.Key).ToList())
            {
                if (!model.UserInDays.Any(x => x.Key == item.Key))
                    model.UserInDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.PaymentLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                x.Value
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.PayCountInDays.Any(x => x.Key == item.Key))
                    model.PayCountInDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.PaymentLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            })
            .OrderBy(x => x.Key).ToList())
            {
                if (!model.PayAmountInDays.Any(x => x.Key == item.Key))
                    model.PayAmountInDays.Add(item.Key, item.Value);
            }
            return model;
        }

        public FinancialStatisticsModel GetFinancial()
        {
            var model = new FinancialStatisticsModel();
            var allItems = _statistic.AsNoTracking()
                .Where(x => x.Type == StaticticsType.AllPayment
                || x.Type == StaticticsType.AllOtherItemPayment
                || x.Type == StaticticsType.AllLicenseItemPayment)
                .OrderByDescending(x => x.InsertDateMi).Take(getCount).ToList();
            var allPayment = allItems.FirstOrDefault(x=>x.Type == StaticticsType.AllPayment);
            model.AllPayment = allPayment == null ? 0 : allPayment.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price;
            var allOtherItemPayment = allItems.FirstOrDefault(x => x.Type == StaticticsType.AllOtherItemPayment);
            model.AllOtherItemPayment = allOtherItemPayment == null ? 0 : allOtherItemPayment.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price;
            var allLicenseItemPayment = allItems.FirstOrDefault(x => x.Type == StaticticsType.AllLicenseItemPayment);
            model.AllLicenseItemPayment = allLicenseItemPayment == null ? 0 : allLicenseItemPayment.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price;
            var items = _statistic.Where(x =>  x.Type == StaticticsType.PaymentLastDays
                                            || x.Type == StaticticsType.OtherItemPaymentLastDays
                                            || x.Type == StaticticsType.LicenseItemPaymentLastDays
                                            || x.Type == StaticticsType.PaymentLastMonths
                                            || x.Type == StaticticsType.OtherItemPaymentLastMonths
                                            || x.Type == StaticticsType.LicenseItemPaymentLastMonths)
                                            .OrderByDescending(x => x.InsertDateMi)
                                            .Take(getCount).ToList();
            foreach (var item in items.Where(x => x.Type == StaticticsType.LicenseItemPaymentLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.LicenseItemPaymentLastDays.Any(x => x.Key == item.Key))
                    model.LicenseItemPaymentLastDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.PaymentLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.PaymentLastDays.Any(x => x.Key == item.Key))
                    model.PaymentLastDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.OtherItemPaymentLastDays).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].InsertDateSh),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.OtherItemPaymentLastDays.Any(x => x.Key == item.Key))
                    model.OtherItemPaymentLastDays.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.PaymentLastMonths).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Mounth),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.PaymentLastMonths.Any(x => x.Key == item.Key))
                    model.PaymentLastMonths.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.LicenseItemPaymentLastMonths).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Mounth),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.LicenseItemPaymentLastMonths.Any(x => x.Key == item.Key))
                    model.LicenseItemPaymentLastMonths.Add(item.Key, item.Value);
            }
            foreach (var item in items.Where(x => x.Type == StaticticsType.OtherItemPaymentLastMonths).Select(x => new
            {
                Key = (string)(x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Mounth),
                Value = x.ExtraData.DeSerializeJson<List<StatisticExtraData>>()[0].Price
            }).OrderBy(x => x.Key).ToList())
            {
                if (!model.OtherItemPaymentLastMonths.Any(x => x.Key == item.Key))
                    model.OtherItemPaymentLastMonths.Add(item.Key, item.Value);
            }
            return model;
        }
    }
}
