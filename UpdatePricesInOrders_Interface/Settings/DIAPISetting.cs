using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace UpdatePricesInOrders_Interface.Settings
{
    public interface IDIAPISetting
    {
        string Server { get; }
        string CompanyDB { get; }
        string DbUserName { get; }
        string DbPassword { get; }
        string UserName { get; }
        string Password { get; }
    }

    class DIAPISetting : IDIAPISetting
    {
        private readonly string _Server;
        private readonly string _CompanyDB;
        private readonly string _DbUserName;
        private readonly string _DbPassword;
        private readonly string _UserName;
        private readonly string _Password;

        public string Server => _Server ?? throw new NotImplementedException();
        public string CompanyDB => _CompanyDB ?? throw new NotImplementedException();
        public string DbUserName => _DbUserName ?? throw new NotImplementedException();
        public string DbPassword => _DbPassword ?? throw new NotImplementedException();
        
        public string UserName => _UserName ?? throw new NotImplementedException();
        public string Password => _Password ?? throw new NotImplementedException();


        public DIAPISetting(EEnvironment env)
        {
            if (env == EEnvironment.PRODUCTION)
            {
                _Server = "WIN-RUB2NQTEU9L";
                _CompanyDB = "GBS";
                _DbUserName = "SA";
                _DbPassword = "EleInitial0";
                _UserName = "manager";
                _Password = "Silla123$";
            }
            else if (env == EEnvironment.DEVELOPMENT)
            {
                _Server = "WIN-RUB2NQTEU9L";
                _CompanyDB = "TEST_SBO_STS2_DB_2024OCT";
                _DbUserName = "SA";
                _DbPassword = "EleInitial0";
                _UserName = "manager";
                _Password = "Silla123$";
            }
        }
    }
}
