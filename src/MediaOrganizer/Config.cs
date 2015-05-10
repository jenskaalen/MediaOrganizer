using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;

namespace MediaOrganizer
{
    public static class Config
    {
        public static bool OverwriteExistingFiles
        {
            get
            {
                try
                {
                    return bool.Parse(ConfigurationManager.AppSettings["OverwriteExistingFiles"]);
                }
                catch (Exception)
                {
                    Logging.Log.Warn("Couldnt find or parse boolean value of OverwriteExistingFiles from the applicationsettings in the config file. Default value is false");
                    return false;
                }
            }
        }

        public const string StorageFolder = "Storage";
    }
}
