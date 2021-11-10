using Quartz;
using Quartz.Impl;
using System;

namespace RasmiOnline.Console.QuartzService
{
    public class JobScheduler
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            var job = JobBuilder.Create<MainJob>().WithIdentity("MainJob").Build();

            var trigger = TriggerBuilder.Create().WithIdentity("MainTrigger")
            .StartAt(DateTimeOffset.Parse("13:00:00"))
            //.StartNow()
            .WithSimpleSchedule(x => x
            //.WithIntervalInSeconds(10)
            //.WithIntervalInHours(2)
            .RepeatForever())
            .Build();

            scheduler.Result.ScheduleJob(job, trigger);

            scheduler.Result.Start();
        }
    }
}