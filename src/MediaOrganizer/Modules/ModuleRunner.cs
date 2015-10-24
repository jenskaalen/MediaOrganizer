using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MediaOrganizer.Scanner.Handlers.Xml;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Modules
{
    /// <summary>
    /// Runs all modules specified in Modules.xml 
    /// </summary>
    public class ModuleRunner
    {
        private const string ModulesFile = "Modules.xml";

        public void RunModules()
        {
            List<IModule> modules = GetModules();

            foreach (IModule module in modules)
            {
                Logging.Log.DebugFormat("Running module {0}", module.Name);
                try
                {
                    module.Start();
                    module.End();
                }
                catch (Exception ex)
                {
                    Logging.Log.Error(ex);
                }
            }
        }

        private List<IModule> GetModules()
        {
            var xdoc = XDocument.Load(ModulesFile);
            List<XElement> modulElements = GetModuleElements(xdoc);

            var modules = new List<IModule>();

            foreach (XElement moduleElement in modulElements)
            {
                try
                {
                    IModule module = GetModule(moduleElement);
                    modules.Add(module);
                }
                catch (Exception ex)
                {
                    Logging.Log.ErrorFormat("Failed loading module from xml element: {0}",ex);
                }
            }

            return modules;
        }

        private List<XElement> GetModuleElements(XDocument xdoc)
        {
            return xdoc.Root.Elements("Module").ToList();
        }

        private string GetModuleType(XElement moduleElement)
        {
            return moduleElement.Attribute("type").Value;
        }

        private IModule GetModule(XElement moduleElement)
        {
            string moduleType = GetModuleType(moduleElement);


            switch (moduleType)
            {
                case "RssDownloader":
                    return ParseRssDownloader(moduleElement);
                case "Unzipper":
                    return ParseUnzipper(moduleElement);
                default:
                    throw new NotImplementedException("module name not supported: " + moduleType );
            }
        }

        private IModule ParseUnzipper(XElement moduleElement)
        {
            string name = moduleElement.Attribute("name").Value;
            var zippingProgram = (ZippingProgram)Enum.Parse(typeof(ZippingProgram), moduleElement.Attribute("unzipperType").Value);
            string application = moduleElement.Element("ApplicationPath").Value;
            string searchDirectory = moduleElement.Element("SearchDirectory").Value;

            var unzipper = new Unzipper(zippingProgram, application, searchDirectory, name);
            return unzipper;
        }

        //TODO: refactor this away
        public RssDownloader ParseRssDownloader(XElement moduleElement)
        {
            string name = moduleElement.Attribute("name").Value;
            string feedUrl = moduleElement.Element("FeedUrl").Value;
            string torrentApplication = moduleElement.Element("TorrentApplicationPath").Value;

            List<IContentMatcher> matchers =
                HandlerXmlParser.ParseContentMatches(moduleElement.Element("MatchPatterns"));
            var matcher = new AnyMatcher(matchers);

            var downloader = new RssDownloader(name, torrentApplication, feedUrl, matcher);
            return downloader;
        }
    }
}
