using Quartz;
using System;
using Gnu.Framework.Core.Log;
using System.Threading.Tasks;
using RasmiOnline.Data.Context;
using System.Threading;

namespace RasmiOnline.Console.QuartzService
{
	public class MainJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
			try
			{
				using (var db = new RasmiDbContext())
				{
					db.Database.ExecuteSqlCommand("EXEC [Statistic].[User]");

					Thread.Sleep(2000);

					db.Database.ExecuteSqlCommand("EXEC [Statistic].[Order]");

					Thread.Sleep(2000);

					db.Database.ExecuteSqlCommand("EXEC [Statistic].[Payment]");
				}

				return Task.CompletedTask;
			}
			catch (Exception e)
			{
				FileLoger.Error(e);
				return Task.CompletedTask;
			}
        }
	}
}