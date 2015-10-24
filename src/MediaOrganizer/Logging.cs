using log4net;
using log4net.Config;

namespace MediaOrganizer
{
    public static class Logging
    {
        private static ILog _log;

        public static ILog Log {
            get
            {
                if (_log == null)
                {
                    XmlConfigurator.Configure();
                    _log = LogManager.GetLogger("MediaOrganizer");
                }

                return _log;
            }
        }
    }
}
