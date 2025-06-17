using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Web;
using Quartz;
using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatePricesInOrders_Interface.Services.SDK;
using UpdatePricesInOrders_Interface.Jobs;

namespace UpdatePricesInOrders_Interface
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);  // si se usa el archivo appsettings.json hay que descomentarlo

            IHost host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {

               services.AddQuartz(q =>
               {
                   q.UseMicrosoftDependencyInjectionScopedJobFactory();

                   var connection = new ConnectionDIAPI();
                   connection.Connect();

                   q.AddJobAndTrigger<Job_UpdatePricesInOrders>(hostContext.Configuration);


                   connection.Disconnect();
                   Console.WriteLine("Servicio Quartz iniciado...");
               });

               services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
           }).ConfigureLogging(logging =>
           {
               logging.ClearProviders();

           }).UseNLog().UseWindowsService().Build();

            await host.RunAsync();
        }
    }
}
