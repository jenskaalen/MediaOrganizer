using System.Diagnostics;
using System.IO;
using MediaOrganizer.Modules;
using MediaOrganizer.Scanner;

namespace MediaOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logging.Log.Info("##START##");
            try
            {

                if (!Directory.Exists(Config.StorageFolder))
                    Directory.CreateDirectory(Config.StorageFolder);

                var moduleRunner = new ModuleRunner();
                moduleRunner.RunModules();
                Logging.Log.Debug("Modules loaded");

                IMediaScanner scanner = new XmlMediaScanner();
                Logging.Log.Debug("Mediascanner loaded");
                scanner.Scan();
            }
            catch (System.Exception ex)
            {
                Logging.Log.Info("Application stopped due to uncaught error", ex);
            }

            Logging.Log.Info("##DONE##");
        }
    }
}
