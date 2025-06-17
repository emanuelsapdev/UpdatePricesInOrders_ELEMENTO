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


namespace UpdatePricesInOrders_Interface.Jobs
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

            _config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            _appSetting = new AppSettings();

            Helpers_Job_UpdatePricesInOrders.Configuration = _config;
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
                    string priceList = _config["PriceListBased"];

                    _oRecorset = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    string qOrds = $@"SELECT T0.DocEntry, T1.ItemCode, T2.Price AS NewPrice, T1.OpenQty, T1.Quantity, T1.LineNum
                                    FROM ORDR T0
                                    INNER JOIN RDR1 T1 
                                    ON T1.DocEntry = T0.DocEntry AND (T1.Quantity = T1.OpenQty OR (T1.Quantity > T1.OpenQty AND T1.OpenQty > 0))
                                    INNER JOIN ITM1 T2 ON T2.ItemCode = T1.ItemCode AND T2.PriceList = {priceList}
                                    WHERE T0.CANCELED = 'N' AND T0.DocStatus = 'O' AND ISNULL(T0.AgrNo, 0) = 0";

                    _oRecorset.DoQuery(qOrds);

                    while (!_oRecorset.EoF)
                    {
                        try
                        {
                            int docEntry = _oRecorset.Fields.Item("DocEntry").Value;
                            string itemCode = _oRecorset.Fields.Item("ItemCode").Value;
                            double newPrice = _oRecorset.Fields.Item("NewPrice").Value;
                            double quantityPending = _oRecorset.Fields.Item("OpenQty").Value;
                            double quantity = _oRecorset.Fields.Item("Quantity").Value;
                            int lineNum = _oRecorset.Fields.Item("LineNum").Value;

                            if (quantity == quantityPending)  // Linea completamente abierta
                            {
                                SAPbobsCOM.Documents oOrder = _company.GetBusinessObject(BoObjectTypes.oOrders);
                                oOrder.GetByKey(docEntry);

                                oOrder.Lines.SetCurrentLine(lineNum);
                                oOrder.Lines.Price = newPrice;

                                oOrder.Update();

                            }
                            else if (quantity > quantityPending) // Linea parcialmente abierta
                            { 
                                // TODO: Cerrar la linea y con esos datos crear un nuevo pedido (Consultar a Fer)
                            }
                        } catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            _log.Error(ex.Message);
                        }
                        finally {
                            _oRecorset.MoveNext();
                        }
                    }

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
