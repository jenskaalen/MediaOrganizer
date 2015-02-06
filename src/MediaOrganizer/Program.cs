using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganizer.Scanner;

namespace MediaOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            IMediaScanner scanner = new StandardMediaScanner();
            scanner.Scan();
        }
    }
}
