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
using UpdatePricesInOrders_Interface.Mapped;


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

            Fn_UpdatePricesInOrders.Configuration = _config;
            Fn_UpdatePricesInOrders.AppSetting = _appSetting;
            Fn_UpdatePricesInOrders.Log = _log;
            Fn_UpdatePricesInOrders.Company = _company;
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
                    _oRecorset = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    string query = "SELECT * FROM ITPS_UPDATE_ORDERS";
                    _oRecorset.DoQuery(query);

                    while (!_oRecorset.EoF)
                    {
                        try
                        {
                            var data = OrderMapped.FromRecordset(_oRecorset);
                            
                            SAPbobsCOM.Documents oOrder = _company.GetBusinessObject(BoObjectTypes.oOrders);
                            oOrder.GetByKey(data.vDocEntry);
                            string msgSuccess = $"Orden Nro: {oOrder.DocNum} actualizada con éxito. Articulo: {data.vItemCode}, Linea: {data.vLineNum + 1}, Precio anterior: $ {data.vCurrPrice}, Nuevo precio: $ {data.vNewPrice}";

                            oOrder.UserFields.Fields.Item("U_ITPS_BaseListName").Value = data.vListName;
                            oOrder.UserFields.Fields.Item("U_ITPS_UpdateDateByListPrice").Value = data.vDateUpdate;

                            if (data.vCurrPrice != data.vNewPrice)
                            {


                                // LINEA SIN ENTREGA
                                if (data.vQuantity == data.vQuantityPending)
                                {
                                    // Asignamos el nuevo precio a la linea abierta
                                    oOrder.Lines.SetCurrentLine(data.vLineNum);
                                    oOrder.Lines.UnitPrice = data.vNewPrice;

                                    if (oOrder.Update() != 0)
                                    {
                                        Console.WriteLine(_company.GetLastErrorDescription());
                                        _log.Error(_company.GetLastErrorDescription());
                                    }
                                    else
                                    {
                                        _log.Info(msgSuccess);
                                    }

                                }       // LINEA CON ENTREGA PARCIAL
                                else if (data.vQuantity > data.vQuantityPending)
                                {
                                    // Cerramos la linea parcial
                                    oOrder.Lines.SetCurrentLine(data.vLineNum);
                                    oOrder.Lines.LineStatus = BoStatus.bost_Close;
                                    string vIndicatorIVA = oOrder.Lines.TaxCode;
                                    double vDiscount = oOrder.Lines.DiscountPercent;
                                    string vWhsCode = oOrder.Lines.WarehouseCode;

                                    if (oOrder.Update() != 0)
                                    {
                                        Console.WriteLine(_company.GetLastErrorDescription());
                                        _log.Error(_company.GetLastErrorDescription());
                                    }

                                    // Agregamos nueva linea al documento
                                    oOrder.Lines.Add();
                                    oOrder.Lines.ItemCode = data.vItemCode;
                                    oOrder.Lines.UnitPrice = data.vNewPrice;
                                    oOrder.Lines.Quantity = data.vQuantityPending;
                                    oOrder.Lines.TaxCode = vIndicatorIVA;
                                    oOrder.Lines.DiscountPercent = vDiscount;
                                    oOrder.Lines.WarehouseCode = vWhsCode;

                                    if (oOrder.Update() != 0)
                                    {
                                        Console.WriteLine(_company.GetLastErrorDescription());
                                        _log.Error(_company.GetLastErrorDescription());
                                    }
                                    else
                                    {
                                        _log.Info(msgSuccess);
                                    }

                                }
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

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
