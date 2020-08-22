namespace RasmiOnline.Console.Controllers
{
    using RasmiOnline.Business.Protocol;
    using System.Web.Mvc;

    //[RoutePrefix("Portal/SellOrder"), Route("{action}")]
    public partial class DashboardController : Controller
    {
        readonly IOfflineStatisticsBusiness _statisticSrv;
        public DashboardController(IOfflineStatisticsBusiness statisticSrv)
        {
            _statisticSrv = statisticSrv;
        }
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult Statistic()
        {
            return View(_statisticSrv.Get());
        }

        [HttpGet]
        public virtual ActionResult FinancialStatistic()
        {
            return View(_statisticSrv.GetFinancial());
        }
    }
}