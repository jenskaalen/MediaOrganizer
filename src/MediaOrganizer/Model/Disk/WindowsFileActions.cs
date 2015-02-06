﻿using System;
using System.IO;

namespace MediaOrganizer.Model.Disk
{
    public class WindowsFileActions : IFileActions
    {
        public string Copy(string filePath, string directory)
        {
            string filename = Path.GetFileName(filePath);
            string newpath = String.Format("{0}\\{1}", directory, filename);
            File.Copy(filePath, newpath, true);
            return newpath;
        }

        public string Copy(string filePath, string directory, string newFilename)
        {
            string newpath = String.Format("{0}\\{1}", directory, newFilename);
            File.Copy(filePath, newpath, true);
            return newpath;
        }

        public string Move(string filePath, string directory)
        {
            string filename = Path.GetFileName(filePath);
            string newpath = String.Format("{0}\\{1}", directory, filename);
            File.Move(filePath, newpath);
            return newpath;
        }

        public string Move(string filePath, string directory, string newFilename)
        {
            string newpath = String.Format("{0}\\{1}", directory, newFilename);
            File.Move(filePath, newpath);
            return newpath;
        }

        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }
    }
}