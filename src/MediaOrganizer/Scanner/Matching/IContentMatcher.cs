namespace MediaOrganizer.Scanner.Matching
{
    public interface IContentMatcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename">Full file name including path and extension</param>
        /// <returns></returns>
        bool Match(string fullFilename);
    }
}
