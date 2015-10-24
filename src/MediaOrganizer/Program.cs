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

            if (!Directory.Exists(Config.StorageFolder))
                Directory.CreateDirectory(Config.StorageFolder);

            var moduleRunner = new ModuleRunner();
            moduleRunner.RunModules();

            IMediaScanner scanner = new XmlMediaScanner();
            scanner.Scan();

            Logging.Log.Info("##DONE##");
        }
    }
}
