using System.Collections.Generic;
using System.Linq;

namespace MediaOrganizer.Scanner.Matching
{
    class FileMatcher : IContentMatcher
    {
        private readonly IEnumerable<IContentMatcher> _matchers;

        public FileMatcher(IEnumerable<IContentMatcher> matchers)
        {
            _matchers = matchers;
        }

        public bool Match(string fullFilename)
        {
            return _matchers.All(matcher => matcher.Match(fullFilename));
        }
    }
}