using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MediaOrganizer.Model.Disk;

namespace MediaOrganizer.Scanner.Handling
{
    public abstract class MediaHandler : IMediaHandler
    {
        public string Name { get; protected set; }
        public List<string> SearchDirectories { get; protected set; }
        public string ContentDirectory { get; set; }
        public List<MediaFile> RegisteredMedia { get; protected set; }
        public IFilenameChanger FilenameChanger { get; protected set; }
        public IFileActions FileActions { get; protected set; }


        public virtual void SearchMedia()
        {
            RegisteredMedia = LoadRegisteredMedia();
        }


        protected List<MediaFile> LoadRegisteredMedia()
        {
            if (!File.Exists(Name + "_media.xml"))
                return new List<MediaFile>();

            using (var writer = new StreamReader(Name + "_media.xml", Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<MediaFile>));
                return (List<MediaFile>)serializer.Deserialize(writer);
            }
        }

        protected void SaveRegisteredMedia(List<MediaFile> registeredMedia)
        {
            using (var writer = new StreamWriter(Name + "_media.xml", false, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<MediaFile>));
                serializer.Serialize(writer, registeredMedia);
            }
        }
    }
}