﻿using Crawler.Data.DbContext;
using Crawler.DTO.RequestDTO;
using Crawler.DTO.ResponseDTO;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using ServiceStack.ServiceClient.Web;
using Sitecrawler.Core.Code;
using Sitecrawler.Core.Interface;
using System;
using System.Collections.Generic;

namespace Crawler.Core.Code
{
    public static class TimeExtension
    {
        public static DateTime Round10(this DateTime value)
        {
            var ticksIn10Mins = TimeSpan.FromMinutes(5).Ticks;

            return (value.Ticks % ticksIn10Mins == 0) ? value : new DateTime((value.Ticks / ticksIn10Mins + 1) * ticksIn10Mins);
        }
    }

    public class TimePeriodManager
    {
        IScheduler scheduler;
        List<IJob> joblist = new List<IJob>();

        public TimePeriodManager()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public void InitTimer()
        {
            scheduler.Start();
        }

        public void ClearTimer()
        {
            scheduler.Clear();

        }
 
        public void AddCrawlers()
        {
            RunJob(joblist);
        }
        public void RunCrawler(List<ICrawler> crawlerList)
        {
            var starttime = DateTime.Now.Round10();
            foreach (var eachCrawler in crawlerList)
            {
            }
        }

        public void RunJob(List<IJob> joblist)
        {
            var starttime = DateTime.Now.Round10();

            foreach (var eachJob in joblist)
            {
                var type = eachJob.GetType();
                var jobdetail =  JobBuilder.Create(type)
                                             .WithIdentity(type.Name, "DefaultJobGroup")
                                             .Build();

                var triggername = type.Name;
                var trigger = TriggerBuilder.Create()
                      .WithIdentity(type.Name + "Trigger", type.Name + "TriggerGroup")
                      .StartAt(starttime)
                      .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                      .Build();

                scheduler.ScheduleJob(jobdetail, trigger);
                Console.WriteLine("Added Crawler" + type.Name);
            }
        }

        public void AddJob(TimePeriodDTO timeperiod, IJob job)
        {
            var type = job.GetType();

            var jobdetail =  scheduler.GetJobDetail(JobKey.Create(type.Name)) 
                            ?? JobBuilder.Create(type)
                                         .WithIdentity(type.Name, "DefaultJobGroup")
                                         .Build();

            var triggername = timeperiod.Label + type.Name;

            var trigger = TriggerBuilder.Create()
                  .WithIdentity(timeperiod.Label+type.Name, type.Name+"TriggerGroup")
                  .StartAt(timeperiod.Scheduled)
                  .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                  .Build();
        }

        public void AddTimeperiod()
        {
            var client = new JsonServiceClient("http://localhost:61910");

            var timeperiods = client.Send<EnvelopeDTO<List<TimePeriodDTO>>>(new TimePeriodCreateRequestDTO
            {
                Scheduled = DateTime.Now.Round10(),
            });
        }

        public List<TimePeriodDTO> ReloadTimePeriod()
        {
            var client = new JsonServiceClient("http://localhost:61910");

            var timeperiods = client.Send<EnvelopeDTO<List<TimePeriodDTO>>>(new TimePeriodGetUpcomingRequestDTO
            {
            });
            
            return timeperiods.ReturnBody;
        }
        
    }
}
