using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using MediaOrganizer.Model.Disk;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handlers.Xml
{
    /// <summary>
    /// Copies to a single ContentDirectory. Best used for movies and the like which do not need to be sorted to differnet subdirectories
    /// </summary>
    public class StandardXmlMediaHandler : IMediaHandler
    {
        public string Name { get; protected set; }
        public List<string> SearchDirectories { get; protected set; }
        public string ContentDirectory { get; set; }
        public IFilenameChanger FilenameChanger { get; protected set; }
        public IFileActions FileActions { get; protected set; }

        public IContentMatcher ContentMatcher { get; private set; }

        private const string DataFolder = "DirectoryStates";

        public StandardXmlMediaHandler(string name, List<string> searchDirectories, IContentMatcher contentMatcher)
        {
            ContentMatcher = contentMatcher;
            SearchDirectories = searchDirectories;
            Name = name;
            FileActions = new WindowsFileActions();

            Initialize();
        }

        public StandardXmlMediaHandler(XElement handlerElement)
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

        public void SearchMedia()
        {
            string[] allSavedMedia = Directory.GetFiles(ContentDirectory, "*", SearchOption.AllDirectories);
            

            foreach (var searchDirectory in SearchDirectories)
            {
                try
                {
                    var files = Directory.GetFiles(searchDirectory, "*", SearchOption.AllDirectories)
                               .Where(file =>
                                   allSavedMedia.All(mediaFile
                               => Path.GetFileName(mediaFile) != Path.GetFileName(file)));

                    foreach (string file in files)
                    {
                        try
                        {
                            if (ContentMatcher.Match(file))
                            {
                                RenameAndCopyFile(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Log.Error("Failure handling file " + file, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Failed handling   directory " + searchDirectory, ex);
                }
            }
        }

        private void RenameAndCopyFiles(IEnumerable<string> filenames)
        {
            foreach (string filename in filenames)
            {
                RenameAndCopyFile(filename);
            }
        }

        private void RenameAndCopyFile(string filename)
        {
            string checkedFile = filename;

            if (FilenameChanger != null)
                checkedFile = FilenameChanger.ChangedName(filename);

            FileActions.Copy(checkedFile, ContentDirectory);

            Logging.Log.InfoFormat("Found match for {0}. Copying to {1}", checkedFile, ContentDirectory);
        }

        private void Initialize()
        {
            try
            {
                if (!Directory.Exists(GetDataFolder()))
                    Directory.CreateDirectory(GetDataFolder());

                if (!File.Exists(GetDataFile()))
                    CreateDataFile();
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Error initializing media handler {0}. {1}", Name, ex);
                throw;
            }
        }

        private string GetDataFolder()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string folderpath = Path.Combine(path, DataFolder);
            return folderpath;
        }

        private string GetDataFile()
        {
            string file = String.Format("{0}.xml", Name);
            string folder = GetDataFolder();
            string completePath = Path.Combine(folder, file);
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
