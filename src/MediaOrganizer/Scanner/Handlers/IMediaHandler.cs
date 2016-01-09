using System.Collections.Generic;
using MediaOrganizer.Model.Disk;

namespace MediaOrganizer.Scanner.Handlers
{
    public interface IMediaHandler
    {
        string Name { get; }
        List<string> SearchDirectories { get; }
        string ContentDirectory { get; set; }
        IFileActions FileActions { get; }
        void SearchMedia();
        void HandleFile(string filename);
    }
}
