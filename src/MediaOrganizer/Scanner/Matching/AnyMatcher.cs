using System.Collections.Generic;
using System.Linq;

namespace MediaOrganizer.Scanner.Matching
{
    class AnyMatcher: IContentMatcher
    {
        public List<IContentMatcher> Matches { get; set; }

        public AnyMatcher(List<IContentMatcher> contentMatchers)
        {
            Matches = contentMatchers;
        }

        public bool Match(string fullFilename)
        {
            return Matches.Any(matcher => matcher.Match(fullFilename));
        }
    }
}
