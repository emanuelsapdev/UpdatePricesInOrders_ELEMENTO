using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePricesInOrders_Interface.Settings
{
    public interface IHanaSetting
    {
        string SBO { get; }
    }


    class HanaSetting : IHanaSetting
    {
        private readonly string _SBO;

        public HanaSetting(EEnvironment env)
        {
            if (env == EEnvironment.PRODUCTION)
            {
                _SBO = "SERVERNODE={192.168.0.166:30015};DSN=HANA;UID=SYSTEM;PWD=CafeInitial0;CS=CAFE_MARTINEZ_ITPS_PROD;databaseName=NDB";
            }
            else if (env == EEnvironment.DEVELOPMENT)
            {
                _SBO = "SERVERNODE={192.168.0.166:30015};DSN=HANA;UID=SYSTEM;PWD=CafeInitial0;CS=BASE_DE_SOPORTESAP;databaseName=NDB";
            }
        }

        public string SBO => _SBO ?? throw new NotImplementedException();
    }
}
