using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganizer.Model;
using MediaOrganizer.Scanner.Handling;

namespace MediaOrganizer.Scanner
{
    interface IMediaScanner
    {
        List<IMediaHandler> Handlers { get; set; }
        void Scan();
    }
}
