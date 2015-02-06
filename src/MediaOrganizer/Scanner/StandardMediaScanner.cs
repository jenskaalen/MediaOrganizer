using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net.Repository.Hierarchy;
using MediaOrganizer.Model;
using MediaOrganizer.Scanner.Handling;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner
{
    public class StandardMediaScanner : IMediaScanner
    {
        public List<IMediaHandler> Handlers { get; set; }
        private bool _initalized;

        public void Initialize()
        {
            LoadHandlers();
        }

        public void Scan()
        {
            if (_initalized == false)
                Initialize();

            if (Handlers == null || !Handlers.Any())
            {
                Logging.Log.Error("Handlers are not loaded, or no handlers were loaded.");
                throw new Exception("Handlers are not loaded, or no handlers were loaded.");
            }

            Handlers.ForEach(RunHandler);
        }

        private void RunHandler(IMediaHandler handler)
        {
            handler.SearchMedia();
        }

        private void LoadHandlers()
        {
            Handlers = new List<IMediaHandler>();

            try
            {
                string handlerSettingsFile = ConfigurationManager.AppSettings["HandlerSettingsFile"];
                XDocument settingsDocument = LoadHandlerDocument(handlerSettingsFile);

                foreach (XElement handlerElement in settingsDocument.Root.Elements("Handler"))
                {
                    string type = handlerElement.Attribute("type").Value;

                    switch (type)
                    {
                        case "ShowMediaHandler":
                            var showHandler = new ShowHandler(handlerElement);
                            Handlers.Add(showHandler);
                            break;
                        default:
                            Logging.Log.WarnFormat("Handler type {0} not found", type);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Failure loading media handlers", ex);
                throw;
            }
        }

        private static XDocument LoadHandlerDocument(string handlerSettingsFile)
        {
            try
            {
                return XDocument.Load(handlerSettingsFile);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Failure loading handler settings file at " + handlerSettingsFile, ex);
                throw;
            }
        }
    }
}
