using System.Text.RegularExpressions;

namespace MediaOrganizer.Scanner.Matching
{
    public class RegexContentMatcher : IContentMatcher
    {
        private Regex _regex;

        public RegexContentMatcher(string regexPattern)
        {
            _regex = new Regex(regexPattern);
        }

        public bool Match(string fullFilename)
        {
            return _regex.IsMatch(fullFilename);
        }
    }
}
