using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
    public enum EEnvironment
    {
        DEVELOPMENT,
        PRODUCTION
    }

namespace UpdatePricesInOrders_Interface.Settings
{
    public interface IAppSettings
    {
        EEnvironment Environment { get; set; }
        IDIAPISetting DIAPI { get; }
        IHanaSetting HanaSetting { get; }

    }

    class AppSettings : IAppSettings
    {
        private IDIAPISetting _DIAPISetting;
        private IHanaSetting _HanaSetting;

        public EEnvironment Environment { get; set; }

        // SINGLENTON
        private static AppSettings _Instance;
        public static AppSettings Singlenton()
        {
            if (_Instance == null)
            {
                _Instance = new AppSettings();
            }
            return _Instance;
        }

        public AppSettings()
        {
            Environment = EEnvironment.DEVELOPMENT;  // CAMBIAR A PRODUCCION CUANDO ESTE COMPLETADO 

            _DIAPISetting = new DIAPISetting(Environment);
            _HanaSetting = new HanaSetting(Environment);

        }

        public IDIAPISetting DIAPI => _DIAPISetting ?? throw new NotImplementedException();
        public IHanaSetting HanaSetting => _HanaSetting ?? throw new NotImplementedException();

    }
}
