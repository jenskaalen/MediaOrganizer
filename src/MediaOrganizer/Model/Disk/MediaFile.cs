using System;

namespace MediaOrganizer.Model.Disk
{
    [Serializable()]
    public class MediaFile
    {
        public string OriginalFilename { get; set; }
        /// <summary>
        /// Filename given by the managed directory
        /// </summary>
        public string ManagedFilename { get; set; }

        public MediaFile()
        {
        }

        public MediaFile(string originalFilename, string managedFilename)
        {
            OriginalFilename = originalFilename;
            ManagedFilename = managedFilename;
        }
    }
}
