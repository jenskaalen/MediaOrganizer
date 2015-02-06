using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MediaOrganizer
{
    public static class Logging
    {
        private static ILog _log;

        public static ILog Log {
            get
            {
                if (_log == null)
                    _log = LogManager.GetLogger("LogManager");

                return _log;
            }
        }
    }
}
