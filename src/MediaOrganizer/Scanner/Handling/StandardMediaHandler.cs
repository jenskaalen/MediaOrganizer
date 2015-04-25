using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MediaOrganizer.Model;
using MediaOrganizer.Model.Disk;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handling
{
    /// <summary>
    /// Copies to a single ContentDirectory. Best used for movies and the like which do not need to be sorted to differnet subdirectories
    /// </summary>
    public class StandardMediaHandler : MediaHandler
    {
        public IContentMatcher ContentMatcher { get; private set; }

        private const string DataFolder = "DirectoryStates";

        public StandardMediaHandler(string name, List<string> searchDirectories, IContentMatcher contentMatcher)
        {
            ContentMatcher = contentMatcher;
            SearchDirectories = searchDirectories;
            Name = name;
            FileActions = new WindowsFileActions();

            Initialize();
        }

        public StandardMediaHandler(XElement handlerElement)
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
                FileActions = new WindowsFileActions();
                ContentMatcher = HandlerXmlParser.ParseContentMatches(handlerElement.Element("MatchPatterns")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Failure loading showhandler", ex);
                throw;
            }
        }

        public override void SearchMedia()
        {
            base.SearchMedia();

            foreach (var directory in SearchDirectories)
            {
                var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
                    .Where(file => 
                        ContentMatcher.Match(file)
                    && RegisteredMedia.All(mediaFile => mediaFile.OriginalFilename != file));

                RenameAndCopyFiles(files);
            }

            SaveRegisteredMedia(RegisteredMedia);
        }

        private void RenameAndCopyFiles(IEnumerable<string> filenames)
        {
            foreach (string filename in filenames)
            {
                string checkedFile = filename;

                if (FilenameChanger != null)
                    checkedFile = FilenameChanger.ChangedName(filename);

                FileActions.Copy(checkedFile, ContentDirectory);
            }
        }

        private void Initialize()
        {
            try
            {
                if (!Directory.Exists(GetDataFolder()))
                    Directory.CreateDirectory(GetDataFolder());

                if (!File.Exists(GetDataFile()))
                    CreateDataFile();

                LoadRegisteredMedia();
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Error initializing media handler {0}. {1}", Name, ex);
                throw;
            }
        }

        private string GetDataFolder()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folderpath = String.Format("{0}\\{1}", path, DataFolder);
            return folderpath;
        }

        private string GetDataFile()
        {
            string file = String.Format("{0}.xml", Name);
            string folder = GetDataFolder();
            string completePath = String.Format("{0}\\{1}", folder, file);
            return completePath;
        }

        private void CreateDataFile()
        {
            var xDoc = new XDocument();
            var container = new XElement("Files");
            xDoc.Add(container);
            Logging.Log.InfoFormat("Creating new datafile for media handler {0}", Name);
        }
    }
}
