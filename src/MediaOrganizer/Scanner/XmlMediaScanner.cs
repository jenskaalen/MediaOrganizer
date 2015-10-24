using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using MediaOrganizer.Scanner.Handlers;
using MediaOrganizer.Scanner.Handlers.Xml;

namespace MediaOrganizer.Scanner
{
    public class XmlMediaScanner : IMediaScanner
    {
        public List<IMediaHandler> Handlers { get; set; }
        private bool _initalized;

        public void Initialize()
        {
            LoadHandlers();
        }

        public void Scan()
        {
            Logging.Log.Debug("Started scan");

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
                    try
                    {
                        string type = handlerElement.Attribute("type").Value;

                        switch (type.ToLower())
                        {
                            case "showmediahandler":
                                var showHandler = new ShowHandler(handlerElement);
                                Handlers.Add(showHandler);
                                break;
                            case "standardmediahandler":
                                var standardHandler = new StandardXmlMediaHandler(handlerElement);
                                Handlers.Add(standardHandler);
                                break;
                            default:
                                Logging.Log.WarnFormat("Handler type {0} not found", type);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.ErrorFormat("Failure loading media handler: {0}. \n Handler file: {1}", ex, handlerElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Failure loading media handlers", ex);
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
