using System;
using System.Diagnostics;
using System.IO;
using MediaOrganizer.Modules;
using MediaOrganizer.Scanner;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = null;

            Logging.Log.Info("##START##");
            try
            {
                if (args.Length > 0)
                {
                    if (!String.IsNullOrEmpty(args[0]))
                    {
                        filename = args[0];
                    }
                }

                var moduleRunner = new ModuleRunner();
                moduleRunner.RunModules();
                Logging.Log.Debug("Modules loaded");

                IMediaScanner scanner = new XmlMediaScanner();
                Logging.Log.Debug("Mediascanner loaded");

                if (!Directory.Exists(Config.StorageFolder))
                    Directory.CreateDirectory(Config.StorageFolder);

                if (!String.IsNullOrEmpty(filename))
                {

                    bool isValidFile = IsValidFile(filename);

                    if (!isValidFile)
                        throw new Exception(filename + " is not a valid file. Scan stopped.");

                    Logging.Log.InfoFormat("Scanning single file {0} ", filename);
                    scanner.Scan(filename);
                }
                else
                {
                    Logging.Log.Debug("Doing a complete scan"); 
                    scanner.Scan();
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Info("Application stopped due to an unhandled error", ex);
            }

            Logging.Log.Info("##DONE##");
        }

        private static bool IsValidFile(string filename)
        {
            try
            {
                FileInfo info = new FileInfo(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
