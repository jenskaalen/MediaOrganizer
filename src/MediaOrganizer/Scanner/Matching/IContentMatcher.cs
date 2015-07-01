namespace MediaOrganizer.Scanner.Matching
{
    public interface IContentMatcher
    {
        /// <summary>
        /// Checks if a file matches the requirements of the matcher
        /// </summary>
        /// <param name="fullFilename">Full file name including path and extension</param>
        /// <returns></returns>
        bool Match(string fullFilename);
    }
}
