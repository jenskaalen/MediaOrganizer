using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganizer.Modules;
using MediaOrganizer.Scanner;

namespace MediaOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists(Config.StorageFolder))
                Directory.CreateDirectory(Config.StorageFolder);

            var moduleRunner = new ModuleRunner();
            moduleRunner.RunModules();

            IMediaScanner scanner = new StandardMediaScanner();
            scanner.Scan();
        }
    }
}
