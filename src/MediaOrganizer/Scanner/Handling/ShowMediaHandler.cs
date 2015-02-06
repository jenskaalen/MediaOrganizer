using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net.Repository.Hierarchy;
using MediaOrganizer.Model.Disk;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handling
{
    public class ShowHandler : IMediaHandler
    {
        public string Name { get; private set; }
        public List<string> SearchDirectories { get; private set; }
        public string ContentDirectory { get; set; }
        public List<MediaFile> RegisteredMedia { get; private set; }
        public IFilenameChanger FilenameChanger { get; private set; }
        public IFileActions FileActions { get; private set; }
        public List<string> Shows { get; private set; }

        private List<ShowMatcher> _showMatches; 

        public ShowHandler(IEnumerable<string> shows)
        {
            Shows = shows.ToList();
            FileActions = new WindowsFileActions();
        }

        public ShowHandler(XElement handlerElement)
        {
            try
            {
                Name = handlerElement.Element("Name").Value;
                ContentDirectory = handlerElement.Element("ContentDirectory").Value;
                SearchDirectories = new List<string>();

                //TODO: this one is volatile, make it safer
                var searchDirectoriesElement = handlerElement.Element("SearchDirectories");

                if (searchDirectoriesElement != null)
                {
                    SearchDirectories =
                        searchDirectoriesElement
                            .Elements("SearchDirectory")
                            .Select(element => element.Value).ToList();
                }
                else
                {
                    SearchDirectories = new List<string>();
                    Logging.Log.WarnFormat("No directores found for {0}", Name);
                }

                //TODO: volatile code, can easily crash. fix it
                _showMatches =  HandlerXmlParser.ParseContentMatches(handlerElement.Element("MatchPatterns")).Cast<ShowMatcher>().ToList();
                Shows = _showMatches.Select(matcher => matcher.Show).ToList();
                FileActions = new WindowsFileActions();
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Failure loading showhandler", ex);
                throw;
            }
        }

        public void SearchMedia()
        {
            InitializeShowDirectories();
            RegisteredMedia = LoadRegisteredMedia();

            foreach (var directory in SearchDirectories)
            {
                var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);

                foreach (ShowMatcher matcher in _showMatches)
                {
                    var matchedFiles = files.
                        Where(file => matcher.Match(file)
                        && RegisteredMedia.All(registeredMedia => registeredMedia.OriginalFilename != file)
                        ).ToList();
                    string show = matcher.Show;

                    if (matchedFiles.Any())
                        RenameAndCopyFiles(matchedFiles, show);
                }
            }

            SaveRegisteredMedia(RegisteredMedia);
        }

        private List<MediaFile> LoadRegisteredMedia()
        {
            if (!File.Exists(Name + "_media.xml"))
                return new List<MediaFile>();

            using (var writer = new StreamReader(Name + "_media.xml", Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<MediaFile>));
                return (List<MediaFile>) serializer.Deserialize(writer);
            }
        }

        private void SaveRegisteredMedia(List<MediaFile> registeredMedia)
        {
            using (var writer = new StreamWriter(Name + "_media.xml", false, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof (List<MediaFile>));
                serializer.Serialize(writer, registeredMedia);
            }
        }

        private void InitializeShowDirectories()
        {
            foreach (string show in Shows)
            {
                string showDirectory = Path.Combine(ContentDirectory, show);

                if (!Directory.Exists(showDirectory))
                    Directory.CreateDirectory(showDirectory);
            }
        }

        private void RenameAndCopyFiles(IEnumerable<string> filenames, string show)
        {
            foreach (string filename in filenames)
            {
                string checkedFile = filename;

                if (FilenameChanger != null)
                    checkedFile = FilenameChanger.ChangedName(filename);

                string newFile = Path.Combine(ContentDirectory, show);
                string copiedFile = FileActions.Copy(checkedFile, newFile);

                RegisteredMedia.Add(
                    new MediaFile(filename, copiedFile)
                    );
            }
        }
    }
}
