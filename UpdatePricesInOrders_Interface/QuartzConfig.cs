using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl;
using Quartz;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace UpdatePricesInOrders_Interface
{
    public static class QuartzConfig
    {
        public static void AddJobAndTrigger<T>(
          this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
          where T : IJob
        {
            //Pegar nome da classe - mesmo nome da chave do config
            string nomeJob = typeof(T).Name;

            var configKey = $"Quartz:{nomeJob}";
            var expressionCron = config[configKey]; 

            //registrando o job
            var jobKey = new JobKey(nomeJob);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(nomeJob + "-trigger")
                .WithCronSchedule(expressionCron));
        }

    }
}
