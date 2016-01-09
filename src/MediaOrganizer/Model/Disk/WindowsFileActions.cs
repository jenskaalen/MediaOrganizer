using System;
using System.IO;

namespace MediaOrganizer.Model.Disk
{
    public class WindowsFileActions : IFileActions
    {
        public string Copy(string filePath, string directory)
        {
            string filename = Path.GetFileName(filePath);
            string newpath = Path.Combine(directory, filename);

            if (!Config.OverwriteExistingFiles && File.Exists(newpath))
            {
                Logging.Log.DebugFormat("{0} already exists. Skipping", newpath);
                return newpath;
            }

            File.Copy(filePath, newpath, Config.OverwriteExistingFiles);
            return newpath;
        }

        public string Copy(string filePath, string directory, string newFilename)
        {
            string newpath = Path.Combine(directory, newFilename);

            if (!Config.OverwriteExistingFiles && File.Exists(newpath))
            {
                Logging.Log.DebugFormat("{0} already exists. Skipping", newpath);
                return newpath;
            }

            File.Copy(filePath, newpath, Config.OverwriteExistingFiles);
            return newpath;
        }

        public string Move(string filePath, string directory)
        {
            string filename = Path.GetFileName(filePath);
            string newpath = Path.Combine(directory, filename);
            File.Move(filePath, newpath);
            return newpath;
        }

        public string Move(string filePath, string directory, string newFilename)
        {
            string newpath = Path.Combine(directory, newFilename);
            File.Move(filePath, newpath);
            return newpath;
        }

        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
