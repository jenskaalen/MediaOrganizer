namespace MediaOrganizer.Model.Disk
{
    public interface IFileActions
    {
        /// <summary>
        /// Copies the file over and keeps the original filename
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="directory"></param>
        string Copy(string filePath, string directory);
        /// <summary>
        /// Copies the file over and renames it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="directory"></param>
        /// <param name="newFilename"></param>
        string Copy(string filePath, string directory, string newFilename);
        /// <summary>
        /// Moves the file over and keeps the original filename
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="directory"></param>
        string Move(string filePath, string directory);
        /// <summary>
        /// Moves the file over and renames it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="directory"></param>
        /// <param name="newFilename"></param>
        string Move(string filePath, string directory, string newFilename);
        /// <summary>
        /// Deletes the files
        /// </summary>
        /// <param name="filePath"></param>
        void Delete(string filePath);
    }
}
