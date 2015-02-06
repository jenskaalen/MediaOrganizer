using System.Collections.Generic;
using MediaOrganizer.Model.Disk;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handling
{
    public interface IMediaHandler
    {
        string Name { get; }
        List<string> SearchDirectories { get; }
        string ContentDirectory { get; set; }
        List<MediaFile> RegisteredMedia { get; }
        /// <summary>
        /// If not set then the filename will not be changed either
        /// </summary>
        IFilenameChanger FilenameChanger { get; }
        IFileActions FileActions { get; }
        void SearchMedia();
    }
}
