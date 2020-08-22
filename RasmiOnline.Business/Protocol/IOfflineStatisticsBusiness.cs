using RasmiOnline.Domain.Dto;

namespace RasmiOnline.Business.Protocol
{
    public interface IOfflineStatisticsBusiness
    {
        StatisticModel Get();

        FinancialStatisticsModel GetFinancial();
    }
}