using System;
using System.Configuration;

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
