﻿using System.Collections.Generic;
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

        protected string MediaStorageFile { get { return Config.StorageFolder + "\\" + Name + "_media.xml"; } }

        public virtual void SearchMedia()
        {
            RegisteredMedia = LoadRegisteredMedia();
        }


        protected List<MediaFile> LoadRegisteredMedia()
        {
            if (!File.Exists(MediaStorageFile))
                return new List<MediaFile>();

            using (var writer = new StreamReader(MediaStorageFile, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<MediaFile>));
                return (List<MediaFile>)serializer.Deserialize(writer);
            }
        }

        protected void SaveRegisteredMedia(List<MediaFile> registeredMedia)
        {
            using (var writer = new StreamWriter(MediaStorageFile, false, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<MediaFile>));
                serializer.Serialize(writer, registeredMedia);
            }
        }
    }
}