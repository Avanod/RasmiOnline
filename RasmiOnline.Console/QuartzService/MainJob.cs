using Quartz;
using System;
using Gnu.Framework.Core.Log;
using System.Threading.Tasks;
using RasmiOnline.Data.Context;
using System.Threading;
using System.Linq;
using RasmiOnline.Domain.Enum;
using RasmiOnline.Business.Implement;
using RasmiOnline.Domain.Entity;
using Gnu.Framework.Core;

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

                    Thread.Sleep(2000);

                    //send message to customers 
                    var dateFrom = DateTime.Now.AddDays(-2).Date;
                    var dateTo = DateTime.Now.AddDays(-1).Date;
                    var orders = db.Order.Where(X => X.OrderStatus == OrderStatus.Done
                    && X.DateOrderDoneMi >= dateFrom
                    && X.DateOrderDoneMi <= dateTo)
                        .ToList();

                    IMessagingStrategy messagingStrategy = new SmsStrategy(db);
                    var sms = db.SmsTemplate.Where(X => X.Key == "Order_Done").FirstOrDefault();
                    if (sms != null)
                        foreach (var item in orders)
                        {
                            var user = db.User.FirstOrDefault(X => X.UserId == item.UserId);
                            var message = new Message
                            {
                                Content = sms.Text,
                                InsertDateMi = DateTime.Now,
                                InsertDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date),
                                Receiver = user.MobileNumber.ToString(),
                                State = StateType.Begin,
                                Type = MessagingType.Sms,
                                 

                            };
                            db.Message.Add(message);
                            db.SaveChanges();
                            messagingStrategy.Send(message);
                        }
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