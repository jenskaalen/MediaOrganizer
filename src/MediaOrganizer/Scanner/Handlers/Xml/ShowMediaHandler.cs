using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MediaOrganizer.Model.Disk;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handlers.Xml
{
    public class ShowHandler : IMediaHandler
    {
        public string Name { get; }
        public List<string> SearchDirectories { get; }
        public string ContentDirectory { get; set; }
        public IFileActions FileActions { get; }

        public List<string> Shows { get; }

        private readonly List<ShowMatcher> _showMatches; 

        public ShowHandler(IEnumerable<string> shows)
        {
            Shows = shows.ToList();
            FileActions = new WindowsFileActions();
        }

        public ShowHandler(XElement handlerElement)
        {
            try
            {
                if (handlerElement?.Element("Name") == null)
                    throw new NullReferenceException(nameof(handlerElement));

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
                _showMatches = new List<ShowMatcher>();
                //HandlerXmlParser.ParseContentMatches(handlerElement.Element("MatchPatterns")).Cast<ShowMatcher>().ToList();

                _showMatches = HandlerXmlParser.ParseContentMatches(handlerElement.Element("MatchPatterns")).Cast<ShowMatcher>().ToList();
               
                Shows = _showMatches.Select(matcher => matcher.Show).ToList();
                Shows = new List<string>();

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
            string[] allSavedMedia = Directory.GetFiles(ContentDirectory, "*", SearchOption.AllDirectories);

            InitializeShowDirectories();

            foreach (var directory in SearchDirectories)
            {
                var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);

                foreach (ShowMatcher matcher in _showMatches)
                {
                    try
                    {
                        var matchedFiles = files.
                                        Where(file => matcher.Match(file)
                                        && allSavedMedia.All(registeredMedia => 
                                        Path.GetFileName(registeredMedia) !=  Path.GetFileName(file))
                                        ).ToList();

                        string show = matcher.Show;

                        if (matchedFiles.Any())
                            RenameAndCopyFiles(matchedFiles, show);
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.Error("Failure running matches against ShowMatcher for " + matcher.Show, ex);
                    }
                }
            }
        }

        public void HandleFile(string filename)
        {
            Logging.Log.DebugFormat("Handling file with {0} showmatches", _showMatches.Count);

            foreach (ShowMatcher matcher in _showMatches)
            {
                try
                {
                    Logging.Log.DebugFormat("Matching against {0}", matcher.Show);
                    string[] allSavedMedia = Directory.GetFiles(ContentDirectory, "*", SearchOption.AllDirectories);

                    bool isMatch = matcher.Match(filename);
                    bool isPreviouslyRegistered = allSavedMedia.Any(registeredMedia =>
                        Path.GetFileName(registeredMedia) == Path.GetFileName(filename));
                    string show = matcher.Show;
                    
                    if (isMatch && !isPreviouslyRegistered)
                    {
                        RenameAndCopyFile(filename, show);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Failure running matches against ShowMatcher for " + matcher.Show, ex);
                }
            }
        }

        private void InitializeShowDirectories()
        {
            foreach (string show in Shows)
            {
                try
                {
                    string showDirectory = Path.Combine(ContentDirectory, show);

                    if (!Directory.Exists(showDirectory))
                        Directory.CreateDirectory(showDirectory);
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Failed creating showdirectory for " + show);
                }
            }
        }

        private void RenameAndCopyFiles(IEnumerable<string> filenames, string show)
        {
            foreach (string filename in filenames)
            {
                RenameAndCopyFile(filename, show);
            }
        }

        private void RenameAndCopyFile(string filename, string show)
        {
            string checkedFile = filename;
            string newFile = Path.Combine(ContentDirectory, show);

            Logging.Log.DebugFormat("Found match for {0} on show {1} and copying to {2}...", checkedFile, show, newFile);
            FileActions.Copy(checkedFile, newFile);
        }
    }
}
