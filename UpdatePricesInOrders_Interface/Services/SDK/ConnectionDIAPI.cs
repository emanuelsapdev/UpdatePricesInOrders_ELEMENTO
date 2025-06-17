using UpdatePricesInOrders_Interface.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePricesInOrders_Interface.Services.SDK
{
    public interface IConnectionDIAPI
    {
        void Connect();
        void Disconnect();
        SAPbobsCOM.Company Company { get; }
    }

    public class ConnectionDIAPI : IConnectionDIAPI
    {
        private SAPbobsCOM.Company _oCompany;
        private IAppSettings _appSettings;
        public SAPbobsCOM.Company Company => _oCompany ?? throw new NotImplementedException();
        public ConnectionDIAPI() 
        {

            _appSettings = new AppSettings();
            if (_oCompany == null)
            {
                _oCompany = new SAPbobsCOM.Company();
                _oCompany.Server = _appSettings.DIAPI.Server;
                _oCompany.CompanyDB = _appSettings.DIAPI.CompanyDB;
                _oCompany.DbUserName = _appSettings.DIAPI.DbUserName;
                _oCompany.DbPassword = _appSettings.DIAPI.DbPassword;
                _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            }
        }

        public void Connect()
        {
            _oCompany.UserName = _appSettings.DIAPI.UserName;
            _oCompany.Password = _appSettings.DIAPI.Password;
            if(_oCompany.Connect() != 0)
            {
                Console.WriteLine($"Error de conexión: {_oCompany.GetLastErrorDescription()}");
            }
        }

        public void Disconnect()
        {
            _oCompany.Disconnect();

        }
    }
}
