using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using UpdatePricesInOrders_Interface.Settings;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using SAPbobsCOM;

namespace UpdatePricesInOrders_Interface.Jobs
{
    public class Helpers_Job_UpdatePricesInOrders
    {
        public static IAppSettings AppSetting {  get; set; }
        public static NLog.ILogger Log {  get; set; }
        public static SAPbobsCOM.Company Company {  get; set; }


        
    }
}
