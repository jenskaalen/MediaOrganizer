using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net.Repository.Hierarchy;
using MediaOrganizer.Scanner;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Modules
{
    public class ModuleRunner
    {
        public void RunModules()
        {
            List<IModule> modules = GetModules();

            foreach (IModule module in modules)
            {
                Logging.Log.DebugFormat("Running module {0}", module.Name);
                module.Start();
                module.End();
            }
        }

        private List<IModule> GetModules()
        {
            var xdoc = XDocument.Load("Modules.xml");
            List<XElement> modulElements = GetModuleElements(xdoc);

            var modules = new List<IModule>();

            foreach (XElement moduleElement in modulElements)
            {
                IModule module = GetModule(moduleElement);
                modules.Add(module);
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
                default:
                    throw new NotImplementedException("module name not supported: " + moduleType );
            }
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
