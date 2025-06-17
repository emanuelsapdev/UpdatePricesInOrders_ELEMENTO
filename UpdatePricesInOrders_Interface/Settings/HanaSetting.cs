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
                _SBO = "SERVERNODE={WIN-RUB2NQTEU9L};DSN=HANA;UID=SA;PWD=EleInitial0;CS=TEST_SBO_STS2_DB_2024OCT;databaseName=NDB";
            }
            else if (env == EEnvironment.DEVELOPMENT)
            {
                _SBO = "SERVERNODE={WIN-RUB2NQTEU9L};DSN=HANA;UID=SA;PWD=EleInitial0;CS=TEST_SBO_STS2_DB_2024OCT;databaseName=NDB";
            }
        }

        public string SBO => _SBO ?? throw new NotImplementedException();
    }
}
