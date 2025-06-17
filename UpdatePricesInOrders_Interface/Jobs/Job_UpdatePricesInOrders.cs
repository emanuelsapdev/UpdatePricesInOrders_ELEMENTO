using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NLog.Filters;
using System.Drawing;
using SAPbobsCOM;
using System.Data.Common;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Quartz;
using System.Web;
using System.Configuration;
using System.IO;
using UpdatePricesInOrders_Interface.Services.SDK;
using UpdatePricesInOrders_Interface.Settings;
using System.Runtime.InteropServices;
using log4net;
using Microsoft.Extensions.Configuration;
using UpdatePricesInOrders_Interface.Tools;


namespace UpdatePricesInOrders_Interface.Job_UpdatePricesInOrders
{

    [DisallowConcurrentExecution]
    public class Job_UpdatePricesInOrders : IJob
    {

        public Task Execute(IJobExecutionContext context)
        { 
            Start();
            return Task.CompletedTask;
        }

        public static IConfigurationRoot _config { get; set; }
        public static IAppSettings _appSetting { get; set; }
        private static SAPbobsCOM.Company _company { get; set; }
        public static NLog.ILogger _log { get; set; }
        private static SAPbobsCOM.Recordset _oRecorset { get; set; }

     
        public Job_UpdatePricesInOrders()
        {
            var connection = new ConnectionDIAPI();
            connection.Connect();
            _company = connection.Company;

            _log = NLog.LogManager.GetCurrentClassLogger();

            //_config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.Build();

            _appSetting = new AppSettings();

            //Helpers_Job1_SendDocsElectronicByMail.Configuration = _config;
            Helpers_Job_UpdatePricesInOrders.AppSetting = _appSetting;
            Helpers_Job_UpdatePricesInOrders.Log = _log;
            Helpers_Job_UpdatePricesInOrders.Company = _company;
        }

        public void Start()
        {
            try
            {
                
                if (!_company.Connected)
                {
                    _log.Error($"Error de conexión: {_company.GetLastErrorDescription()}");
                } 

                else
                {
                   
                    // code..
                    
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally 
            {

                _company.Disconnect();
                MarshalGC.ReleaseComObjects(_oRecorset, _company, _log, _appSetting);
                GC.WaitForPendingFinalizers();

            }

        }
    }
}
