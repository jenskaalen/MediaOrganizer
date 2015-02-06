using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaOrganizer.Scanner.Matching
{
    public class MultipleShowsMatcher : IContentMatcher
    {
        private readonly List<string> _shows;
        private readonly RegexContentMatcher _regexMatcher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shows">Shownames</param>
        /// <param name="regexPattern">Matching pattern</param>
        public MultipleShowsMatcher(IEnumerable<string> shows, string regexPattern)
        {
            _regexMatcher = new RegexContentMatcher(regexPattern);
            _shows = shows.ToList();
        }

        public bool Match(string fullFilename)
        {
            if (!_regexMatcher.Match(fullFilename))
                return false;

            return MatchesAgainstShow(fullFilename) != null;
        }

        public string MatchesAgainstShow(string fullFilename)
        {

            string filename = Path.GetFileName(fullFilename);
            filename = filename.Replace(".", " ");

            string[] words = filename.Split(' ');

            return _shows.FirstOrDefault(show =>
                words.All(word => show.Contains(show))
                );
        }
    }
}
