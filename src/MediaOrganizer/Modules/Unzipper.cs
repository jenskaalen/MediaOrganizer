using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;

namespace MediaOrganizer.Modules
{
    public class Unzipper: IModule
    {
        private readonly ZippingProgram _zippingProgram;
        private readonly string _programPath;
        private readonly string _searchFolder;
        private string PreviousUnzipsFile { get { return Config.StorageFolder + "\\" + Name + ".txt"; } }
        private List<string> _previouslyUnzippedFiles;
        public string Name { get; set; }

        private bool _anyUnzipsPerformed;

        public Unzipper(ZippingProgram zippingProgram, string programPath, string searchFolder, string name)
        {
            Name = name;
            _zippingProgram = zippingProgram;
            _programPath = programPath;
            _searchFolder = searchFolder;
        }

        public void Start()
        {
            List<string> filesToCheck= System.IO.Directory.GetFiles(_searchFolder, "*.rar", SearchOption.AllDirectories).ToList();
            string[] zipFiles = System.IO.Directory.GetFiles(_searchFolder, "*.zip", SearchOption.AllDirectories);
            filesToCheck.AddRange(zipFiles);

            if (!File.Exists(PreviousUnzipsFile))
            {
                File.Create(PreviousUnzipsFile);   
                _previouslyUnzippedFiles = new List<string>();
            }
            else
            {
                _previouslyUnzippedFiles = File.ReadLines(PreviousUnzipsFile).ToList();   
            }

            for (int i = 0; i < filesToCheck.Count; i++)
            {
                if (_previouslyUnzippedFiles.Contains(filesToCheck[i]))
                    continue;
                try
                {

                    Unzip(filesToCheck[i]);
                }
                catch (Exception ex)
                {
                    Logging.Log.Error(ex);
                }
            }
        }

        public void End()
        {
            if (_anyUnzipsPerformed)
                File.WriteAllLines(PreviousUnzipsFile, _previouslyUnzippedFiles);
        }

        public void Unzip(string file)
        {
            Logging.Log.DebugFormat("Starting unzipping of {0}", file);
            string unzipDirectory = Path.GetDirectoryName(file);

            switch (_zippingProgram)
            {
                case ZippingProgram.SevenZip:
                var process = new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = _programPath,
                        Arguments = "e " + file + " -y",
                        WorkingDirectory = unzipDirectory
                    }
                };
                process.Start();
                process.WaitForExit();
                break;

                default:
                    throw new Exception("Zipping program type not supported");
                    break;
            }

            _anyUnzipsPerformed = true;
            _previouslyUnzippedFiles.Add(file);
            Logging.Log.DebugFormat("Finished unzipping {0}", file);
        }
    }

    public enum ZippingProgram
    {
        SevenZip,
        SevenZipCommandLine
    }
}
