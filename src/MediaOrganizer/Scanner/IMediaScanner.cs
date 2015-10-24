using System.Collections.Generic;
using MediaOrganizer.Scanner.Handlers;

namespace MediaOrganizer.Scanner
{
    interface IMediaScanner
    {
        List<IMediaHandler> Handlers { get; set; }
        void Scan();
    }
}
